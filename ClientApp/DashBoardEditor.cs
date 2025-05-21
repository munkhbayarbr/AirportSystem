using System.Windows.Forms;
using ClinetApp.DTO;
using System.Drawing;
using System.Collections.Generic;

namespace ClientApp
{
    public partial class DashBoardEditor : Form
    {
        private DataGridView arrivalsDataGridView;
        private DataGridView departuresDataGridView;
        private SplitContainer splitContainer;

        public DashBoardEditor()
        {
            InitializeComponent();
            InitializeDataGridViews();

            try
            {
                ConnectionService.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ConnectionService.AllFlight += OnAllFlightsReceived;
            ConnectionService.FlightStatusUpdate += OnFlightStatusUpdated;

            this.FormClosing += DashBoardEditor_FormClosing;
        }

        private async void DashBoardEditor_FormClosing(object? sender, FormClosingEventArgs e)
        {
            await ConnectionService.Stop();
        }

        private void InitializeDataGridViews()
        {
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };
            this.Controls.Add(splitContainer);

            arrivalsDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGreen,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            splitContainer.Panel1.Controls.Add(arrivalsDataGridView);
            splitContainer.Panel1.Text = "Arrivals";

            departuresDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightSalmon,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            splitContainer.Panel2.Controls.Add(departuresDataGridView);
            splitContainer.Panel2.Text = "Departures";

            ArrivalColumns(arrivalsDataGridView);
            DepartureColumns(departuresDataGridView);
        }

        private void ArrivalColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FlightNumber", "Flight #");
            dgv.Columns.Add("Departure", "From");
            dgv.Columns.Add("ArrivalTime", "Time");
            dgv.Columns.Add("Status", "Status");

            dgv.Columns["Status"].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
        }

        private void DepartureColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FlightNumber", "Flight #");
            dgv.Columns.Add("Arrival", "To");
            dgv.Columns.Add("DepartureTime", "Time");
            dgv.Columns.Add("Status", "Status");

            dgv.Columns["Status"].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
        }

        private void OnAllFlightsReceived(IEnumerable<FlightReadDTO> flights)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadFlights(flights)));
            }
            else
            {
                LoadFlights(flights);
            }
        }

        private void OnFlightStatusUpdated(FlightReadDTO flight)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateFlight(flight)));
            }
            else
            {
                UpdateFlight(flight);
            }
        }

        private void LoadFlights(IEnumerable<FlightReadDTO> flights)
        {
            arrivalsDataGridView.Rows.Clear();
            departuresDataGridView.Rows.Clear();

            foreach (var flight in flights)
            {
                
                if (flight.Arrival == "Ulaanbaatar")
                {
                    var dgv = arrivalsDataGridView;
                    dgv.Rows.Add(
                    flight.FlightNumber,
                    flight.Departure,
                    flight.ArrivalTime.ToString("HH:mm"),
                    flight.Status
                );
                }
                else
                {
                    var dgv = departuresDataGridView;
                    dgv.Rows.Add(
                    flight.FlightNumber,
                    flight.Arrival,
                    flight.DepartureTime.ToString("HH:mm"),
                    flight.Status
                );
                }


                
            }
        }

        private void UpdateFlight(FlightReadDTO flight)
        {

            var dgv = flight.Arrival == "Ulaanbaatar" ? arrivalsDataGridView : departuresDataGridView;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["FlightNumber"].Value?.ToString() == flight.FlightNumber)
                {
                    row.Cells["Status"].Value = flight.Status;
                    
                    if (flight.Status.Contains("Delayed") || flight.Status.Contains("Canceled"))
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    break;
                }
            }
        }

        

    }
}