using ClinetApp.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using System;

using System.Collections.Generic;

using System.Drawing;

using System.Drawing.Printing;

using System.Linq;

using System.Net.Http;

using System.Net.Http.Json;

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
        private PassengerReadDTO Passenger;
        private readonly Dictionary<int, Button> _seatButtons = new();

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
                var passenger = await _httpClient
                    .GetFromJsonAsync<PassengerReadDTO>($"passenger/{passengerId}");
                if (passenger == null)
                {
                    MessageBox.Show("Passenger олдсонгүй.");
                    return;
                }

                
                lblFname.Text ="Нэр:"+ passenger.Firstname;
                lblLastName.Text = "Овог:" + passenger.Lastname;
                lblBday.Text = "Төрсөн өдөр:" + passenger.Birthday.ToString("yyyy-MM-dd");
                lblPhone.Text = "Утасны дугаар:" + passenger.PhoneNumber;
               

                
                if (_currentBooking.SeatNumber.HasValue)
                    lblBookedSeat.Text = $"Суудал: {_currentBooking.SeatNumber.Value}";
                else
                    lblBookedSeat.Text = "Суудал хараахан олгогдоогүй";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger уншихад алдаа: {ex.Message}");
            }
        }
        private async Task ShowFlightInfo(int flightId)
        {
            var f = await _httpClient
                .GetFromJsonAsync<FlightReadDTO>($"flight/{flightId}");
            if (f == null) { MessageBox.Show("Flight олдсонгүй."); return; }

            var info =
               $"ID:            {f.Id}\n" +
               $"Number:        {f.FlightNumber}\n" +
               $"Status:        {f.Status}\n" +
               $"From → To:     {f.Departure} → {f.Arrival}\n" +
               $"Departs at:    {f.DepartureTime:yyyy-MM-dd HH:mm}\n" +
               $"Arrives at:    {f.ArrivalTime:yyyy-MM-dd HH:mm}\n" +
               $"Total Seats:   {f.SeatCount}";
            MessageBox.Show(info, "Flight Info");
        }
        private void DrawAirplaneLayout()

        {

            panelSeats.Controls.Clear();

            const int seatsPerRow = 6;

            

            var rowIndices = _seatList

                .Select(s => (s.SeatNumber - 1) / seatsPerRow)

                .Distinct()

                .OrderBy(i => i)

                .ToList();

            var tbl = new TableLayoutPanel
            {

                Dock = DockStyle.Top,

                AutoSize = true,

                ColumnCount = 8,   

                RowCount = rowIndices.Count + 1

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

               
                int col = indexInBlock < 3

                    ? indexInBlock + 1

                    : indexInBlock + 2;

                bool taken = _takenSeatNumbers.Contains(seat.SeatNumber);

                var btn = new Button
                {

                    Text = seat.SeatNumber.ToString(),

                    Width = _buttonSize,

                    Height = _buttonSize,

                    Enabled = !taken,

                    BackColor = taken ? Color.Red : Color.LightGray,

                    FlatStyle = FlatStyle.Flat

                };
                _seatButtons[seat.SeatNumber] = btn;

                if (!taken)

                    btn.Click += SeatButton_Click;

                tbl.Controls.Add(btn, col, tableRow);

            }

            panelSeats.Controls.Add(tbl);

            panelSeats.AutoScroll = true;

        }



        private void AddSeatButton(TableLayoutPanel tbl, int seatNumber, int row, int col)

        {

            if (!_seatList.Any(s => s.SeatNumber == seatNumber))

                return;

            bool taken = _takenSeatNumbers.Contains(seatNumber);

            var btn = new Button

            {

                Text = seatNumber.ToString(),

                Width = _buttonSize,

                Height = _buttonSize,

                Enabled = !taken,

                BackColor = taken ? Color.Red : Color.LightGray,

                FlatStyle = FlatStyle.Flat

            };

            if (!taken)

                btn.Click += SeatButton_Click;

            tbl.Controls.Add(btn, col, row);

        }

        private async void SeatButton_Click(object sender, EventArgs e)

        {

            if (!(sender is Button btn) || !int.TryParse(btn.Text, out var seatNumber))

                return;

            var update = new BookingRecord

            {

                Id = _currentBooking.Id,

                PassengerId = _currentBooking.PassengerId,

                FlightId = _currentBooking.FlightId,

                BookingDate = _currentBooking.BookingDate,

                SeatNumber = seatNumber

            };

            try

            {

                var resp = await _httpClient.PutAsJsonAsync("booking", update);

                if (resp.IsSuccessStatusCode)

                {

                    btn.BackColor = Color.Red;

                    btn.Enabled = false;

                    MessageBox.Show($"Seat {seatNumber} амжилттай оноогдлоо!");
                  

                        var dr = MessageBox.Show("Boarding pass хэвлэх үү?", "Print Boarding Pass",

                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)

                    {

                        // _previewDialog.ShowDialog();

                        if (_printDialog.ShowDialog() == DialogResult.OK)

                            _printDoc.Print();

                    }

                }

                else

                {
                    var apiMsg = await resp.Content.ReadAsStringAsync();
                    MessageBox.Show(apiMsg);

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show($"Алдаа: {ex.Message}");

            }

        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)

        {

            var g = e.Graphics;

            var fontTitle = new Font("Arial", 16, FontStyle.Bold);

            var fontBody = new Font("Arial", 10);

            g.DrawString("BOARDING PASS", fontTitle, Brushes.Black, 100, 50);

            g.DrawString($"Passenger ID: {_currentBooking.PassengerId}", fontBody, Brushes.Black, 100, 100);

            g.DrawString($"Flight ID:     {_currentBooking.FlightId}", fontBody, Brushes.Black, 100, 120);

            g.DrawString($"Seat Number:   {_currentBooking.SeatNumber}", fontBody, Brushes.Black, 100, 140);

            g.DrawString($"Date:          {_currentBooking.BookingDate:yyyy-MM-dd}", fontBody, Brushes.Black, 100, 160);

            g.DrawRectangle(Pens.Black, 90, 40, 300, 150);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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

