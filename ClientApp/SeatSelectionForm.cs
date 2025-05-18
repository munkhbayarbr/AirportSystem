using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class SeatSelectionForm : Form
    {
        // Серверийн холболт
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5106/")
        };

        // Паспортын захиалга
        private BookingRecord _currentBooking;

        // Нислэгийн бүх суудал
        private List<SeatRecord> _seatList = new();

        // Авсан суудлууд
        private HashSet<int> _takenSeatNumbers = new();

        // Суудлын товчнууд
        private readonly Dictionary<int, Button> _seatButtons = new();

        // Тогтмолүүд
        private const int _buttonSize = 40;
        private const int _padding = 5;
        private const int _cols = 6;  // эгнээнд суудал

        public SeatSelectionForm()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click;
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            // Паспорт унших
            if (!int.TryParse(txtPassport.Text.Trim(), out var passportId))
            {
                MessageBox.Show("Зөв паспорт дугаар оруулна уу.");
                return;
            }

            // Захиалга авах
            var bookings = await _httpClient
                .GetFromJsonAsync<List<BookingRecord>>($"booking/getByPassportId/{passportId}");
            if (bookings == null || bookings.Count == 0)
            {
                MessageBox.Show("Booking олдсонгүй.");
                return;
            }
            _currentBooking = bookings[0];

            // Суудлууд авах
            _seatList = await _httpClient
                .GetFromJsonAsync<List<SeatRecord>>($"seat/{_currentBooking.FlightId}")
                ?? new List<SeatRecord>();

            // Захиалга шүүх
            var allBookings = await _httpClient
                .GetFromJsonAsync<List<BookingRecord>>("booking")
                ?? new List<BookingRecord>();

            _takenSeatNumbers = allBookings
                .Where(b => b.FlightId == _currentBooking.FlightId && b.SeatNumber.HasValue)
                .Select(b => b.SeatNumber!.Value)
                .ToHashSet();


            // Суудал зурах
            DrawSeatGrid();
        }

        private void DrawSeatGrid()
        {
            panelSeats.Controls.Clear();
            _seatButtons.Clear();

            int totalSeats = _seatList.Count;
            int rows = (int)Math.Ceiling((double)totalSeats / _cols);

            for (int i = 0; i < totalSeats; i++)
            {
                var seat = _seatList[i];
                int r = i / _cols;
                int c = i % _cols;

                var btn = new Button
                {
                    Width = _buttonSize,
                    Height = _buttonSize,
                    Left = c * (_buttonSize + _padding),
                    Top = r * (_buttonSize + _padding),
                    Text = seat.SeatNumber.ToString()
                };

                if (_takenSeatNumbers.Contains(seat.SeatNumber))
                {
                    btn.BackColor = Color.Red;
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.LightGray;
                    btn.Click += SeatButton_Click;
                }

                panelSeats.Controls.Add(btn);
                _seatButtons[seat.SeatNumber] = btn;
            }

            panelSeats.AutoScroll = true;
        }

        private async void SeatButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !int.TryParse(btn.Text, out var seatNumber))
                return;

            // Өгөгдөл үүсгэх
            var update = new BookingRecord
            {
                Id = _currentBooking.Id,
                PassengerId = _currentBooking.PassengerId,
                FlightId = _currentBooking.FlightId,
                BookingDate = _currentBooking.BookingDate,
                SeatNumber = seatNumber
            };

            var resp = await _httpClient.PutAsJsonAsync("booking", update);
            if (resp.IsSuccessStatusCode)
            {
                btn.BackColor = Color.Red;
                btn.Enabled = false;
                MessageBox.Show($"Seat {seatNumber} амжилттай оноогдлоо!");
            }
            else
            {
                MessageBox.Show("Суудал оноох үед алдаа гарлаа. Дахин оролдоно уу.");
            }
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
