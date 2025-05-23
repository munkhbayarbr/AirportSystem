using ClinetApp.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

using System.Collections.Generic;

using System.Drawing;

using System.Drawing.Printing;

using System.Linq;

using System.Net.Http;

using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ClientApp

{

    public partial class SeatSelectionForm : Form

    {

        private readonly HttpClient _httpClient = new()

        {

            BaseAddress = new Uri("http://localhost:5106/"),

            Timeout = TimeSpan.FromSeconds(10)

        };

        private BookingRecord _currentBooking;

        private List<SeatRecord> _seatList = new();

        private HashSet<int> _takenSeatNumbers = new();

        private const int _buttonSize = 40;

        private readonly PrintDocument _printDoc = new PrintDocument();

        private readonly PrintDialog _printDialog = new PrintDialog();

        private readonly PrintPreviewDialog _previewDialog = new PrintPreviewDialog();
        private PassengerReadDTO passenger;
        private readonly Dictionary<int, Button> _seatButtons = new();
        
        private FlightReadDTO _currentFlight;

        private HubConnection _hubConnection;

        public SeatSelectionForm()

        {

            InitializeComponent();

            btnSearch.Click += BtnSearch_Click;


            _printDoc.PrintPage += PrintDoc_PrintPage;

            _printDialog.Document = _printDoc;

            _previewDialog.Document = _printDoc;

            this.Load += SeatSelectionForm_Load;

            try
            {
                ConnectionService.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ConnectionService.SeatUpdate += seatUpdate;

            this.FormClosing += SeatSelection_FormClosing;


        }
        private async void SeatSelectionForm_Load(object sender, EventArgs e)
        {
            await ConnectToSignalR();
        }

        private void MarkSeatAsBooked(int seatNumber)
        {
            if (_seatButtons.TryGetValue(seatNumber, out var btn))
            {
                if (btn.InvokeRequired)
                {
                    btn.Invoke(new Action(() =>
                    {
                        btn.BackColor = Color.Red;
                        btn.Enabled = false;
                    }));
                }
                else
                {
                    btn.BackColor = Color.Red;
                    btn.Enabled = false;
                }
            }
        }

        private async void SeatSelection_FormClosing(object? sender, FormClosingEventArgs e)
        {
            await ConnectionService.Stop();
        }

        public void seatUpdate(int flightId, int seatNumber)
        {

        }

        public void click_Dashboard(object sender, EventArgs e)
        {
            DashBoardEditor editor = new DashBoardEditor();
            editor.ShowDialog();
        }

        private async void BtnSearch_Click(object sender, EventArgs e)

        {

            btnSearch.Enabled = false;

            try

            {

                if (!int.TryParse(txtPassport.Text.Trim(), out var passportId))

                {

                    MessageBox.Show("Зөв паспорт дугаар оруулна уу.");

                    return;

                }

                var bookings = await _httpClient

                    .GetFromJsonAsync<List<BookingRecord>>($"booking/getByPassportId/{passportId}")

                    ?? new List<BookingRecord>();

                if (bookings.Count == 0)

                {

                    MessageBox.Show("Booking олдсонгүй.");

                    return;

                }

                //Passenger = await _httpClient.GetFromJsonAsync<PassengerReadDTO>($"passanger/{bookings[0].PassengerId}");
                //MessageBox.Show($"Passenger: {Passenger.Firstname}, Passport: {Passenger.PassportId}");

                _currentBooking = bookings[0];

                await LoadAndShowPassenger(_currentBooking.PassengerId);
                await ShowFlightInfo(_currentBooking.FlightId);

                _seatList = await _httpClient

                    .GetFromJsonAsync<List<SeatRecord>>($"seat/{_currentBooking.FlightId}")

                    ?? new List<SeatRecord>();

                var allBookings = await _httpClient

                    .GetFromJsonAsync<List<BookingRecord>>("booking")

                    ?? new List<BookingRecord>();

                _takenSeatNumbers = allBookings

                    .Where(b => b.FlightId == _currentBooking.FlightId && b.SeatNumber.HasValue)

                    .Select(b => b.SeatNumber!.Value)

                    .ToHashSet();

                DrawAirplaneLayout();

                if (_hubConnection?.State == HubConnectionState.Connected)
                {
                    await _hubConnection.InvokeAsync("JoinFlightGroup", _currentBooking.FlightId);
                }


            }

            catch (Exception ex)

            {

                MessageBox.Show($"Алдаа гарлаа: {ex.Message}");

            }

            finally

            {

                btnSearch.Enabled = true;

            }

        }
        private async Task ConnectToSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5106/seathub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<int, int>("ReceiveSeatBooked", (flightId, seatNumber) =>
            {
                if (_currentBooking != null && _currentBooking.FlightId == flightId)
                {
                    MarkSeatAsBooked(seatNumber);
                }
            });

            await _hubConnection.StartAsync();

        }

        private async Task LoadAndShowPassenger(int passengerId)
        {
            try
            {
                passenger = await _httpClient.GetFromJsonAsync<PassengerReadDTO>($"passenger/{passengerId}");


                if (passenger == null)
                {
                    MessageBox.Show("Passenger олдсонгүй.");
                    return;
                }


                lblName.Text = "Нэр :" + passenger.Firstname + " " + passenger.Lastname;
                lblBday.Text = "Төрсөн өдөр: " + passenger.Birthday.ToString("yyyy-MM-dd");
                lblPhone.Text = "Утасны дугаар: " + passenger.PhoneNumber;



                if (_currentBooking.SeatNumber.HasValue)
                {
                    lblBookedSeat.Text = $"Суудал: {_currentBooking.SeatNumber.Value}";
                    lblBookedSeat.BackColor = Color.Green;
                }
                else
                {
                    lblBookedSeat.Text = "Суудал хараахан олгогдоогүй";
                    lblBookedSeat.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger уншихад алдаа: {ex.Message}");
            }
        }
        private async Task ShowFlightInfo(int flightId)
        {
            var f = await _httpClient.GetFromJsonAsync<FlightReadDTO>($"flight/{flightId}");
            if (f == null)
            {
                MessageBox.Show("Flight олдсонгүй.");
                return;
            }

            _currentFlight = f; 
        }

        private void DrawAirplaneLayout()
{
    panelSeats.Controls.Clear();

    var container = new FlowLayoutPanel
    {
        FlowDirection = FlowDirection.TopDown,
        AutoSize = true,
        Dock = DockStyle.Top,
        WrapContents = false
    };


    if (_currentFlight != null)
    {
        var flightInfoLabel = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Padding = new Padding(5),
            ForeColor = Color.DarkBlue,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = $"✈ {_currentFlight.FlightNumber} | {_currentFlight.Departure} → {_currentFlight.Arrival} | " +
                   $"Departure: {_currentFlight.DepartureTime:yyyy-MM-dd HH:mm}"
        };

        container.Controls.Add(flightInfoLabel);
    }

  
    const int seatsPerRow = 6;

    var rowIndices = _seatList
        .Select(s => (s.SeatNumber - 1) / seatsPerRow)
        .Distinct()
        .OrderBy(i => i)
        .ToList();

    var tbl = new TableLayoutPanel
    {
        AutoSize = true,
        ColumnCount = 8,
        RowCount = rowIndices.Count + 1,
        Margin = new Padding(0, 10, 0, 10)
    };

    tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
    for (int i = 0; i < 3; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
    tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20));
    for (int i = 0; i < 3; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

    foreach (var seat in _seatList)
    {
        int zeroBasedSeat = seat.SeatNumber - 1;
        int rowBlock = zeroBasedSeat / seatsPerRow;
        int indexInBlock = zeroBasedSeat % seatsPerRow;
        int tableRow = rowIndices.IndexOf(rowBlock) + 1;
        int col = indexInBlock < 3 ? indexInBlock + 1 : indexInBlock + 2;

        bool taken = _takenSeatNumbers.Contains(seat.SeatNumber);

        var btn = new Button
        {
            Text = seat.SeatNumber.ToString(),
            Width = _buttonSize,
            Height = _buttonSize,
            Enabled = !taken,
            BackColor = taken ? Color.Red : Color.LightGray,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(5)
        };

        _seatButtons[seat.SeatNumber] = btn;
        if (!taken) btn.Click += SeatButton_Click;

        tbl.Controls.Add(btn, col, tableRow);
    }

    container.Controls.Add(tbl);

    panelSeats.Controls.Add(container);
    panelSeats.AutoScroll = true;
}



        private async Task<(bool success, string message)> SendBookSeatViaTcp(int seatNumber)
        {
            try
            {
                var bookSeatData = new
                {
                    action = "bookSeat",
                    data = new
                    {
                        Id = _currentBooking.Id,
                        PassengerId = _currentBooking.PassengerId,
                        FlightId = _currentBooking.FlightId,
                        SeatNumber = seatNumber,
                        BookingDate = _currentBooking.BookingDate
                    }
                };

                var json = JsonConvert.SerializeObject(bookSeatData);
                var bytes = Encoding.UTF8.GetBytes(json);

                using var client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 6000);
                using var stream = client.GetStream();

                await stream.WriteAsync(bytes, 0, bytes.Length);
                await stream.FlushAsync();

                var buffer = new byte[4096];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    var responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var response = JObject.Parse(responseJson);

                    bool success = response["success"]?.Value<bool>() ?? false;
                    string message = response["message"]?.ToString() ?? "No message";

                    return (success, message);
                }

                return (false, "Сервер хариу илгээгээгүй.");
            }
            catch (Exception ex)
            {
                return (false, $"TCP холболт амжилтгүй: {ex.Message}");
            }
        }




        private async void SeatButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !int.TryParse(btn.Text, out var seatNumber))
                return;

            var confirm = MessageBox.Show(
                $"Та {seatNumber} дугаартай суудлыг сонгохдоо итгэлтэй байна уу?",
                "Суудал сонгох баталгаажуулалт",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            var (success, message) = await SendBookSeatViaTcp(seatNumber);

            MessageBox.Show(message);

            if (!success) return;

          
            btn.BackColor = Color.Red;
            btn.Enabled = false;
            lblBookedSeat.Text = $"Суудал: {seatNumber}";
            lblBookedSeat.BackColor = Color.Green;

            var dr = MessageBox.Show("Boarding pass хэвлэх үү?", "Print Boarding Pass",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes && _printDialog.ShowDialog() == DialogResult.OK)
            {
                _printDoc.Print();
            }
        }



        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            int left = 50;
            int top = 40;

            var fontTitle = new Font("Arial", 18, FontStyle.Bold);
            var fontSubTitle = new Font("Arial", 12, FontStyle.Bold);
            var fontText = new Font("Arial", 10);
            var blackBrush = Brushes.Black;

       
            g.DrawRectangle(Pens.Black, left - 10, top - 10, 600, 220);

 
            g.DrawString("BOARDING PASS", fontTitle, blackBrush, left + 180, top);
            top += 40;

         
            g.DrawString("Passenger Name: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{passenger.Firstname} {passenger.Lastname}", fontText, blackBrush, left + 150, top);
            top += 25;

            g.DrawString("Passport No: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{txtPassport.Text}", fontText, blackBrush, left + 150, top);
            top += 25;

       
            g.DrawString("Flight: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{_currentFlight.FlightNumber}", fontText, blackBrush, left + 150, top);
            top += 25;

            g.DrawString("Route: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{_currentFlight.Departure} → {_currentFlight.Arrival}", fontText, blackBrush, left + 150, top);
            top += 25;

            g.DrawString("Departure: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{_currentFlight.DepartureTime:yyyy-MM-dd HH:mm}", fontText, blackBrush, left + 150, top);
            top += 25;

            g.DrawString("Seat Number: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{lblBookedSeat.Text}", fontText, blackBrush, left + 150, top);
            top += 25;

            g.DrawString("Booking Date: ", fontSubTitle, blackBrush, left, top);
            g.DrawString($"{_currentBooking.BookingDate:yyyy-MM-dd}", fontText, blackBrush, left + 150, top);
        }

    }

    public class BookingRecord

    {

        public int Id { get; set; }

        public int PassengerId { get; set; }

        public int FlightId { get; set; }

        public DateTime BookingDate { get; set; }

        public int? SeatNumber { get; set; }

    }

    public class SeatRecord

    {

        public int Id { get; set; }

        public int FlightId { get; set; }

        public int SeatNumber { get; set; }

    }

}

