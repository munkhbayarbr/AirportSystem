using ClinetApp.DTO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class DashBoardEditor : Form
    {
        private DataGridView arrivalsDataGridView;
        private DataGridView departuresDataGridView;
        private SplitContainer splitContainer;
        private List<FlightReadDTO> flights;
        private List<string> fstatuses = new List<string>();

        /// <summary>
        /// Husnegtuudiig uusgej connection service tei holbono.
        /// </summary>
        public DashBoardEditor()
        {
            InitializeComponent();
            InitializeDataGridViews();
            fstatuses.AddRange(new[]{ "On Time", "Boarding", "Cancelled", "Departed", "Landed", "Delayed", "Flying" });
           
            
            /// main luu shiljuulne !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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


        /// <summary>
        /// Dashboardiin medeelliig haruulah husnegtuudiig uusgene.
        /// </summary>
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
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            arrivalsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            arrivalsDataGridView.CellClick += DataGridView_RowClick;
            splitContainer.Panel1.Controls.Add(arrivalsDataGridView);
            splitContainer.Panel1.Text = "Arrivals";

            departuresDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            departuresDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            departuresDataGridView.CellClick += DataGridView_RowClick;
            splitContainer.Panel2.Controls.Add(departuresDataGridView);
            splitContainer.Panel2.Text = "Departures";

            ArrivalColumns(arrivalsDataGridView);
            DepartureColumns(departuresDataGridView);
            splitContainer.SplitterDistance = splitContainer.Width / 2;

        }
        /// <summary>
        /// husnegtuuddee baganuudaa oruulna.
        /// </summary>
        /// <param name="dgv"></param>
        private void ArrivalColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FlightNumber", "Flight #");
            dgv.Columns.Add("Departure", "From");
            dgv.Columns.Add("ArrivalTime", "Time");
            dgv.Columns.Add("Status", "Status");

            dgv.Columns["Status"].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
        }


        /// <summary>
        /// husnegtuuddee baganuudaa oruulna.
        /// </summary>
        /// <param name="dgv"></param>
        private void DepartureColumns(DataGridView dgv)
        {
            dgv.Columns.Add("FlightNumber", "Flight #");
            dgv.Columns.Add("Arrival", "To");
            dgv.Columns.Add("DepartureTime", "Time");
            dgv.Columns.Add("Status", "Status");

            dgv.Columns["Status"].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
        }
        /// <summary>
        /// buh nislegiig husnegtend haruulah functsiig duudna.
        ///
        /// </summary>
        /// <param name="flights"></param>
        private void OnAllFlightsReceived(IEnumerable<FlightReadDTO> flights)
        {
            this.flights = flights.ToList();
            if (InvokeRequired)
            {
                Invoke(new Action(() => LoadFlights(flights)));
            }
            else
            {
                LoadFlights(flights);
            }
        }


        /// <summary>
        /// update hiisen nislegiig oorchilj shinechilsen statusiig haruulah functsiig duudna.
        /// </summary>
        /// <param name="flight"></param>
        private void OnFlightStatusUpdated(FlightReadDTO flight)
        {
            var existingFlight = flights.FirstOrDefault(flgt => flgt.Id == flight.Id);
            if (existingFlight != null)
            {
                flights.Remove(existingFlight);
                flights.Add(flight);
            }

            if (InvokeRequired)
            {
                Invoke(() => UpdateFlight(flight));
            }
            else
            {
                UpdateFlight(flight);
            }
        }

        /// <summary>
        /// buh nislegiig delgetsend haruulna.
        /// </summary>
        /// <param name="flights"></param>
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
        /// <summary>
        /// update hiisen nislegiig haruulna.
        /// </summary>
        /// <param name="flight"></param>
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

        /// <summary>
        /// dashboardiin moriig darhad status solih panel iig gargaj irne.
        /// connection service eer damjuulan nislegiin statusiig solino.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void DataGridView_RowClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            string flightNumber = row.Cells["FlightNumber"].Value?.ToString();
            string status = row.Cells["Status"].Value?.ToString();

            Panel panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(400, 270),
                BackColor = Color.White,
                Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 270) / 2)
            };

            Label title = new Label
            {
                Text = "Flight Details",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panel.Controls.Add(title);

            Label labelFN = new Label
            {
                Text = "Flight number: " + flightNumber,
                Location = new Point(20, 60),
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            };
            panel.Controls.Add(labelFN);

            Label labelST = new Label
            {
                Text = "Flight status: " + status,
                Location = new Point(20, 100),
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            };
            panel.Controls.Add(labelST);

            FlowLayoutPanel sts = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                BackColor = Color.LightGray,
                WrapContents = false,
                Width = 150,
                Height = 180,
                Location = new Point(220, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            foreach (string st in fstatuses)
            {
                Label lbl = new Label
                {
                    Text = st,
                    Font = new Font("Segoe UI", 9),
                    Width = 150,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(130, 25),
                    Cursor = Cursors.Hand,
                    BackColor = Color.White,
                    Margin = new Padding(5)
                };

                lbl.MouseEnter += (s, e) => { lbl.BackColor = Color.Blue; };
                lbl.MouseLeave += (s, e) => { lbl.BackColor = Color.White; };

                lbl.Click += (s, e) =>
                {
                    status = st;
                    labelST.Text = "Flight status: " + st;
                };

                sts.Controls.Add(lbl);
            }
            panel.Controls.Add(sts);

            Button ok = new Button
            {
                Text = "OK",
                Size = new Size(80, 30),
                Location = new Point(290, 210),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            ok.Click += (sender, e) => {

                var upflight = flights.FirstOrDefault(flgt => flgt.FlightNumber == flightNumber);
                var flightJson = new JObject
                {
                    ["action"] = "updateFlight",
                    ["data"] = new JObject
                    {
                        ["Id"] = upflight.Id,
                        ["FlightNumber"] = upflight.FlightNumber,
                        ["Status"] = status,
                        ["Departure"] = upflight.Departure,
                        ["Arrival"] = upflight.Arrival,
                        ["DepartureTime"] = upflight.DepartureTime,
                        ["ArrivalTime"] = upflight.ArrivalTime,
                        ["SeatCount"] = upflight.SeatCount,
                    }
                };

                string jsonString = flightJson.ToString();

                ConnectionService.SendUpdateToServer(jsonString);


                this.Controls.Remove(panel); 
            };
            panel.Controls.Add(ok);

            Button cancel = new Button
            {
                Text = "Cancel",
                Size = new Size(80, 30),
                Location = new Point(200, 210),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };
            cancel.Click += (sender, e) => { this.Controls.Remove(panel); };
            panel.Controls.Add(cancel);

            this.Controls.Add(panel);
            panel.BringToFront();


        }







    }
}