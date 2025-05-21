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
        // HTTP client to your API
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5106/"),
            Timeout = TimeSpan.FromSeconds(10)
        };

        // Current booking & seat data
        private BookingRecord _currentBooking;
        private List<SeatRecord> _seatList = new();
        private HashSet<int> _takenSeatNumbers = new();

        private const int _buttonSize = 40;

        public SeatSelectionForm()
        {
            InitializeComponent();
            btnSearch.Click += BtnSearch_Click;
        }

        private async void BtnSearch_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            try
            {
                // 1) Parse passport
                if (!int.TryParse(txtPassport.Text.Trim(), out var passportId))
                {
                    MessageBox.Show("Зөв паспорт дугаар оруулна уу.");
                    return;
                }

                // 2) Fetch bookings for passport
                var bookings = await _httpClient
                    .GetFromJsonAsync<List<BookingRecord>>($"booking/getByPassportId/{passportId}")
                    ?? new List<BookingRecord>();

                if (bookings.Count == 0)
                {
                    MessageBox.Show("Booking олдсонгүй.");
                    return;
                }

                _currentBooking = bookings[0];


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

        private void DrawAirplaneLayout()
        {
            panelSeats.Controls.Clear();

            var rows = _seatList
                .Select(s => s.SeatNumber / 10)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 8,   
                RowCount = rows.Count + 1,
            };


            tbl.ColumnStyles.Clear();
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // row numbers
            for (int i = 0; i < 3; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20)); // aisle
            for (int i = 0; i < 3; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            // Header row:   [ ]  A   B   C      D   E   F
            tbl.Controls.Add(new Label { Text = "", AutoSize = true }, 0, 0);
            tbl.Controls.Add(new Label { Text = "A", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tbl.Controls.Add(new Label { Text = "B", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 2, 0);
            tbl.Controls.Add(new Label { Text = "C", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 3, 0);
            tbl.Controls.Add(new Label { Text = "" }, 4, 0);
            tbl.Controls.Add(new Label { Text = "D", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 5, 0);
            tbl.Controls.Add(new Label { Text = "E", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 6, 0);
            tbl.Controls.Add(new Label { Text = "F", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter }, 7, 0);

            // Populate each row
            for (int r = 0; r < rows.Count; r++)
            {
                int rowNum = rows[r];
                int tableRow = r + 1;

                // 1) Row label
                tbl.Controls.Add(new Label
                {
                    Text = rowNum.ToString(),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = _buttonSize
                }, 0, tableRow);

                // 2) Seats A,B,C  = rowNum*10 + 1..3
                AddSeatButton(tbl, rowNum * 10 + 1, tableRow, 1);
                AddSeatButton(tbl, rowNum * 10 + 2, tableRow, 2);
                AddSeatButton(tbl, rowNum * 10 + 3, tableRow, 3);

                // 3) aisle at col 4 left blank

                // 4) Seats D,E,F  = rowNum*10 + 4..6
                AddSeatButton(tbl, rowNum * 10 + 4, tableRow, 5);
                AddSeatButton(tbl, rowNum * 10 + 5, tableRow, 6);
                AddSeatButton(tbl, rowNum * 10 + 6, tableRow, 7);
            }

            panelSeats.Controls.Add(tbl);
            panelSeats.AutoScroll = true;
        }

        private void AddSeatButton(TableLayoutPanel tbl, int seatNumber, int row, int col)
        {
            // Only add a button if this seat actually exists in the list
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
                }
                else
                {
                    MessageBox.Show("Суудал оноох үед алдаа гарлаа. Дахин оролдоно уу.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Алдаа: {ex.Message}");
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
