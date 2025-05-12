using Newtonsoft.Json.Bson;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using System.IO;
using System.Windows.Forms;




namespace ClientApp
{
    public partial class Form1 : Form
    {
        public List<string> passengers = new List<string>();
        public List<Button> seats = new List<Button>();
        public bool valid = false;
        public int seatRow = 0;
        public int seatCol = 0;
        public List<FlightInfo> flights = new List<FlightInfo>();
        public Panel info = new Panel();
        public Panel panel = new Panel();
        public TableLayoutPanel table;

        HubConnection connection;
        TcpClient client;
        private NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
            passengers.Add("1");
            passengers.Add("2");
            passengers.Add("3");
            passengers.Add("4");
            passengers.Add("5");

            client = new TcpClient();
            info.Size = new Size(panel1.Width, panel1.Height);
            info.Visible = false;
            info.Location = panel1.Location;
            info.BackColor = Color.LightGray;

            panel.Size = new Size(400, 400);
            panel.Visible = false;
            panel.Location = new Point(panel1.Width/2 + 50, 10);

            TextBox number = new TextBox();
            number.Location = new Point(10, 10);

            TextBox status = new TextBox();
            status.Location = new Point(10, 70);

            Button change = new Button();
            change.Text = "change";
            change.Location = new Point(10, 130);
            change.Click += (sender, e) => { click_Change(number.Text, status.Text); };


            panel.Controls.Add(change);
            panel.Controls.Add(status);
            panel.Controls.Add(number);

            Button edit = new Button();
            edit.Text = "edit";
            edit.AutoSize = true;
            edit.Location = new Point(900, 500);
            edit.Click += click_Edit;


            info.Controls.Add(edit);
            info.Controls.Add(panel);

            this.Controls.Add(info);

            this.Load += Form1_Load;
        }


        public async void click_Change(string number, string status)
        {
            try
            {
                if (!client.Connected)
                {
                    MessageBox.Show("TCP Client is not connected.");
                    return;
                }

                if (stream == null)
                {
                    stream = client.GetStream();
                }

                string[] words = { number, status };
                string joinedWords = string.Join("|", words);
                byte[] stringBytes = Encoding.UTF8.GetBytes(joinedWords);

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.WriteByte(2); // type 2 = string array
                    ms.Write(BitConverter.GetBytes(stringBytes.Length), 0, 4); // length
                    ms.Write(stringBytes, 0, stringBytes.Length);

                    byte[] finalData = ms.ToArray();
                    await stream.WriteAsync(finalData, 0, finalData.Length);
                }

                Console.WriteLine("Sent status with type to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending status : {ex.Message}");
            }
        }

        public void click_Edit(object sender, EventArgs e)
        {

            if (panel.Visible == false)
            {
                panel.Visible = true;
            }
            else
            {
                panel.Visible = false;
            }
            
        }




        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(5000);
                await client.ConnectAsync("127.0.0.1", 6000);
                
                stream = client.GetStream();



                Console.WriteLine("SignalR connected.");

                connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7132/flighthub")
                .WithAutomaticReconnect()
                .Build();

                connection.On<string, string>("ReceiveFlightStatusUpdate", (flightId, newStatus) =>
                {
                    if (table == null) return;

                    if (table.InvokeRequired)
                    {
                        table.Invoke(() => UpdateFlightStatus(flightId, newStatus));
                    }
                    else
                    {
                        UpdateFlightStatus(flightId, newStatus);
                    }
                });


                connection.On<string, string>("ReceiveMessage", (flightId, message) =>
                {
                    MessageBox.Show(message);
                });

                connection.On<string>("ReceiveHello", (message) =>
                {
                    MessageBox.Show(message);
                });


                connection.On<List<FlightInfo>>("ReceiveInfo", (flights) =>
                {
                    //MessageBox.Show(flights.Count.ToString());
                    this.flights = flights;
                    load_Flights();
                    

                });


                await connection.StartAsync();
                //await connection.InvokeAsync("RequestMessage");

                await connection.InvokeAsync("RequestFlightList");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not connect to server: {ex.Message}");
            }
        }

        private void UpdateFlightStatus(string flightId, string newStatus)
        {
            foreach (Control control in table.Controls)
            {
                int index = table.Controls.GetChildIndex(control);

                if (index % 3 == 0) // Only Flight ID column
                {
                    if (control is Label flightLabel && flightLabel.Text == flightId)
                    {
                        int statusIndex = index + 1;
                        if (statusIndex < table.Controls.Count)
                        {
                            if (table.Controls[statusIndex] is Label statusLabel)
                            {
                                statusLabel.Text = newStatus;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public void click_Order(object sender, EventArgs e)
        {
            info.Visible = false;
            panel1.Visible = true;
        }

        public void click_Status(object sender, EventArgs e)
        {
            panel1.Visible=false;
            info.Visible = true;
        }

        public void load_Flights()
        {
            int rowHeight = 50;
            int totalRows = flights.Count + 1; // +1 for header
            int tableHeight = (totalRows * rowHeight)+5;

            table = new TableLayoutPanel
            {
                Size = new Size(panel1.Width / 2, tableHeight), // Width depends on panel, Height is EXACT
                ColumnCount = 3,
                Location = new Point(0, 0),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                AutoSize = false, // Important: Do not AutoSize
                Dock = DockStyle.None, // Important: Do not dock
                Anchor = AnchorStyles.Top | AnchorStyles.Left, // Only anchor top-left
            };

            // Set column styles
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34f));

            table.RowCount = totalRows;

            // Add header row
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            table.Controls.Add(new Label { Text = "Flight Number", TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Arial", 10, FontStyle.Bold) });
            table.Controls.Add(new Label { Text = "Status", TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Arial", 10, FontStyle.Bold) });
            table.Controls.Add(new Label { Text = "Time", TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Arial", 10, FontStyle.Bold) });

            // Add data rows
            foreach (var flight in flights)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                table.Controls.Add(new Label { Text = flight.Number, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill });
                table.Controls.Add(new Label { Text = flight.Status, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill });
                table.Controls.Add(new Label { Text = flight.DateTime.ToString("g"), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill });
            }

            info.Controls.Add(table);
        }



        private void CreatePlaneSeats()
        {
            int rows = 10;
            int seatsPerSide = 3;
            int buttonWidth = 50;
            int buttonHeight = 50;
            int spacing = 10;
            int aisleSpacing = 30;

            panel2.Controls.Clear();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < seatsPerSide * 2 + 1; col++)
                {
                    
                    if (col == seatsPerSide)
                    {
                        continue;
                    }

                    Button seatButton = new Button();
                    seatButton.Width = buttonWidth;
                    seatButton.Height = buttonHeight;
                    seatButton.BackColor = Color.White;

                    int leftOffset = col * (buttonWidth + spacing);
                    if (col > seatsPerSide)
                    {
                        
                        leftOffset += aisleSpacing;
                    }

                    seatButton.Left = leftOffset;
                    seatButton.Top = row * (buttonHeight + spacing);
                    int c = 0;
                    if (col>3)
                    {
                        c = col - 1;
                    }
                    else
                    {
                        c = col;
                    }

                    seatButton.Tag = new SeatLocation
                    {
                        Row = row+1,
                        Column = c+1,
                    };
                    seatButton.Click += click_Seat;
                    seats.Add(seatButton);
                    panel2.Controls.Add(seatButton);
                }
            }
        }

        



        public void click_Search(object sender, EventArgs e)
        {
            var id = textBox1.Text;
            for (int i = 0; i < passengers.Count; i++)
            {
                if (passengers[i] == id)
                {
                    label1.Text = "flight: " + passengers[i];
                    valid = true;
                    CreatePlaneSeats();
                    break;
                }
            }
        }

        public void click_Seat(object sender, EventArgs e)
        {
            Button seat = (Button)sender;
            SeatLocation st = (SeatLocation)seat.Tag;
            int row = st.Row;
            int col = st.Column;
            for (int i = 0; i<seats.Count;i++)
            {
                SeatLocation sts = (SeatLocation)seats[i].Tag;
                if (sts.Row == row && sts.Column == col)
                {
                    seats[i].BackColor = Color.Red;
                    seatRow = sts.Row;
                    seatCol = sts.Column;
                }
                else
                {
                    seats[i].BackColor= Color.White;
                }
            }


        }

        public async void click_Save(object sender, EventArgs e)
        {
            if (!valid)
            {
                MessageBox.Show("Invalid");
                return;
            }

            try
            {
                if (!client.Connected)
                {
                    MessageBox.Show("TCP Client is not connected.");
                    return;
                }

                if (stream == null)
                {
                    stream = client.GetStream();
                }

                int[] nums = { seatRow, seatCol };
                byte[] numbersBytes = new byte[nums.Length * sizeof(int)];

                for (int i = 0; i < nums.Length; i++)
                {
                    byte[] numberBytes = BitConverter.GetBytes(nums[i]);
                    Array.Copy(numberBytes, 0, numbersBytes, i * sizeof(int), sizeof(int));
                }

                // NEW: Create final message with type indicator
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.WriteByte(1); // Write type 1 = integer array
                    ms.Write(numbersBytes, 0, numbersBytes.Length);

                    byte[] finalMessage = ms.ToArray();
                    await stream.WriteAsync(finalMessage, 0, finalMessage.Length);
                }

                Console.WriteLine("Sent seat selection with type to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending seat selection: {ex.Message}");
            }
        }


        public void CloseClientConnection()
        {
            if (client.Connected)
            {
                client.Close();  // Close the TCP connection
                client.Dispose();  // Dispose the client object
                Console.WriteLine("Client connection closed.");
            }
        }

        private async void form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            CloseClientConnection();    

            if (connection != null)
            {
                await connection.StopAsync();
                await connection.DisposeAsync();
            }

            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
            
        }






    }


}
