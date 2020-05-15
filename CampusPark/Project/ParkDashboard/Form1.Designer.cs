namespace Admin_Dashboard
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button btn_GetParkingSpotsSpecificTimePeriod;
            this.btn_GetAllParksOrParkingSpots = new System.Windows.Forms.Button();
            this.text_ParkID = new System.Windows.Forms.TextBox();
            this.text_ParkName = new System.Windows.Forms.TextBox();
            this.text_ParkDescription = new System.Windows.Forms.TextBox();
            this.text_ParkOperatingHours = new System.Windows.Forms.TextBox();
            this.text_ParkGeoLocationFile = new System.Windows.Forms.TextBox();
            this.label_ParkID = new System.Windows.Forms.Label();
            this.label_ParkName = new System.Windows.Forms.Label();
            this.label_ParkDescription = new System.Windows.Forms.Label();
            this.label_NumberOfSpots = new System.Windows.Forms.Label();
            this.label_OperatingHours = new System.Windows.Forms.Label();
            this.label_NumberOfSpecialSpots = new System.Windows.Forms.Label();
            this.label_GeoLocationFile = new System.Windows.Forms.Label();
            this.comboBox_All = new System.Windows.Forms.ComboBox();
            this.btn_GetByIDorByName = new System.Windows.Forms.Button();
            this.text_ByID = new System.Windows.Forms.TextBox();
            this.label_ByID = new System.Windows.Forms.Label();
            this.list_ByID = new System.Windows.Forms.ListBox();
            this.comboBox_NameOrId = new System.Windows.Forms.ComboBox();
            this.label_SpotBatteryStatus = new System.Windows.Forms.Label();
            this.label_SpotTimestamp = new System.Windows.Forms.Label();
            this.label_SpotStatus = new System.Windows.Forms.Label();
            this.label_SpotType = new System.Windows.Forms.Label();
            this.label_SpotSpotID = new System.Windows.Forms.Label();
            this.label_SpotParkID = new System.Windows.Forms.Label();
            this.label_SpotID = new System.Windows.Forms.Label();
            this.text_SpotSpotID = new System.Windows.Forms.TextBox();
            this.text_SpotID = new System.Windows.Forms.TextBox();
            this.text_SpotLocation = new System.Windows.Forms.TextBox();
            this.label_SpotLocation = new System.Windows.Forms.Label();
            this.groupBox_Parks = new System.Windows.Forms.GroupBox();
            this.numericUpDown_ParkNumberOfSpecialSpots = new System.Windows.Forms.NumericUpDown();
            this.btn_ParkDELETE = new System.Windows.Forms.Button();
            this.numericUpDown_ParkNumberOfSpots = new System.Windows.Forms.NumericUpDown();
            this.btn_ParkPUT = new System.Windows.Forms.Button();
            this.btn_ParkPOST = new System.Windows.Forms.Button();
            this.groupBox_ParkingSpots = new System.Windows.Forms.GroupBox();
            this.textBox_SpotParkID = new System.Windows.Forms.TextBox();
            this.comboBox_SpotType = new System.Windows.Forms.ComboBox();
            this.btn_SpotDELETE = new System.Windows.Forms.Button();
            this.btn_SpotPUT = new System.Windows.Forms.Button();
            this.btn_SpotPOST = new System.Windows.Forms.Button();
            this.comboBox_SpotBatteryStatus = new System.Windows.Forms.ComboBox();
            this.comboBox_SpotStatus = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_SpotDateTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.text_ByName = new System.Windows.Forms.TextBox();
            this.dateTime_SpecificMoment = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTime_Start = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTime_End = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_GetParkingSpotsSpecificMoment = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.text_ParkByName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.text_ParkByID = new System.Windows.Forms.TextBox();
            this.btn_GetParkingSpotsSpecificPark = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_GetParkingSpotsFromParkSpecificMoment = new System.Windows.Forms.Button();
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod = new System.Windows.Forms.Button();
            this.list_All = new System.Windows.Forms.ListBox();
            this.comboBox_selectType = new System.Windows.Forms.ComboBox();
            this.comboBox_NameOrIdByPark = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            btn_GetParkingSpotsSpecificTimePeriod = new System.Windows.Forms.Button();
            this.groupBox_Parks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParkNumberOfSpecialSpots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParkNumberOfSpots)).BeginInit();
            this.groupBox_ParkingSpots.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_GetParkingSpotsSpecificTimePeriod
            // 
            btn_GetParkingSpotsSpecificTimePeriod.Location = new System.Drawing.Point(15, 412);
            btn_GetParkingSpotsSpecificTimePeriod.Name = "btn_GetParkingSpotsSpecificTimePeriod";
            btn_GetParkingSpotsSpecificTimePeriod.Size = new System.Drawing.Size(118, 23);
            btn_GetParkingSpotsSpecificTimePeriod.TabIndex = 56;
            btn_GetParkingSpotsSpecificTimePeriod.Text = "Get Parking Spots";
            btn_GetParkingSpotsSpecificTimePeriod.UseVisualStyleBackColor = true;
            btn_GetParkingSpotsSpecificTimePeriod.Click += new System.EventHandler(this.btn_GetParkingSpotsSpecificTimePeriod_Click);
            // 
            // btn_GetAllParksOrParkingSpots
            // 
            this.btn_GetAllParksOrParkingSpots.Location = new System.Drawing.Point(142, 25);
            this.btn_GetAllParksOrParkingSpots.Name = "btn_GetAllParksOrParkingSpots";
            this.btn_GetAllParksOrParkingSpots.Size = new System.Drawing.Size(160, 23);
            this.btn_GetAllParksOrParkingSpots.TabIndex = 0;
            this.btn_GetAllParksOrParkingSpots.Text = "Get All Parks/Parking Spots";
            this.btn_GetAllParksOrParkingSpots.UseVisualStyleBackColor = true;
            this.btn_GetAllParksOrParkingSpots.Click += new System.EventHandler(this.btn_GetAllParksOrParkingSpots_Click);
            // 
            // text_ParkID
            // 
            this.text_ParkID.Location = new System.Drawing.Point(137, 13);
            this.text_ParkID.Name = "text_ParkID";
            this.text_ParkID.Size = new System.Drawing.Size(100, 20);
            this.text_ParkID.TabIndex = 7;
            // 
            // text_ParkName
            // 
            this.text_ParkName.Location = new System.Drawing.Point(137, 39);
            this.text_ParkName.Name = "text_ParkName";
            this.text_ParkName.Size = new System.Drawing.Size(100, 20);
            this.text_ParkName.TabIndex = 8;
            // 
            // text_ParkDescription
            // 
            this.text_ParkDescription.Location = new System.Drawing.Point(137, 65);
            this.text_ParkDescription.Name = "text_ParkDescription";
            this.text_ParkDescription.Size = new System.Drawing.Size(100, 20);
            this.text_ParkDescription.TabIndex = 9;
            // 
            // text_ParkOperatingHours
            // 
            this.text_ParkOperatingHours.Location = new System.Drawing.Point(137, 117);
            this.text_ParkOperatingHours.Name = "text_ParkOperatingHours";
            this.text_ParkOperatingHours.Size = new System.Drawing.Size(100, 20);
            this.text_ParkOperatingHours.TabIndex = 11;
            // 
            // text_ParkGeoLocationFile
            // 
            this.text_ParkGeoLocationFile.Location = new System.Drawing.Point(137, 169);
            this.text_ParkGeoLocationFile.Name = "text_ParkGeoLocationFile";
            this.text_ParkGeoLocationFile.Size = new System.Drawing.Size(100, 20);
            this.text_ParkGeoLocationFile.TabIndex = 13;
            // 
            // label_ParkID
            // 
            this.label_ParkID.AutoSize = true;
            this.label_ParkID.Location = new System.Drawing.Point(6, 16);
            this.label_ParkID.Name = "label_ParkID";
            this.label_ParkID.Size = new System.Drawing.Size(18, 13);
            this.label_ParkID.TabIndex = 14;
            this.label_ParkID.Text = "ID";
            // 
            // label_ParkName
            // 
            this.label_ParkName.AutoSize = true;
            this.label_ParkName.Location = new System.Drawing.Point(6, 42);
            this.label_ParkName.Name = "label_ParkName";
            this.label_ParkName.Size = new System.Drawing.Size(35, 13);
            this.label_ParkName.TabIndex = 15;
            this.label_ParkName.Text = "Name";
            // 
            // label_ParkDescription
            // 
            this.label_ParkDescription.AutoSize = true;
            this.label_ParkDescription.Location = new System.Drawing.Point(6, 68);
            this.label_ParkDescription.Name = "label_ParkDescription";
            this.label_ParkDescription.Size = new System.Drawing.Size(60, 13);
            this.label_ParkDescription.TabIndex = 16;
            this.label_ParkDescription.Text = "Description";
            // 
            // label_NumberOfSpots
            // 
            this.label_NumberOfSpots.AutoSize = true;
            this.label_NumberOfSpots.Location = new System.Drawing.Point(7, 94);
            this.label_NumberOfSpots.Name = "label_NumberOfSpots";
            this.label_NumberOfSpots.Size = new System.Drawing.Size(86, 13);
            this.label_NumberOfSpots.TabIndex = 17;
            this.label_NumberOfSpots.Text = "Number of Spots";
            // 
            // label_OperatingHours
            // 
            this.label_OperatingHours.AutoSize = true;
            this.label_OperatingHours.Location = new System.Drawing.Point(7, 120);
            this.label_OperatingHours.Name = "label_OperatingHours";
            this.label_OperatingHours.Size = new System.Drawing.Size(84, 13);
            this.label_OperatingHours.TabIndex = 18;
            this.label_OperatingHours.Text = "Operating Hours";
            // 
            // label_NumberOfSpecialSpots
            // 
            this.label_NumberOfSpecialSpots.AutoSize = true;
            this.label_NumberOfSpecialSpots.Location = new System.Drawing.Point(7, 146);
            this.label_NumberOfSpecialSpots.Name = "label_NumberOfSpecialSpots";
            this.label_NumberOfSpecialSpots.Size = new System.Drawing.Size(124, 13);
            this.label_NumberOfSpecialSpots.TabIndex = 19;
            this.label_NumberOfSpecialSpots.Text = "Number of Special Spots";
            // 
            // label_GeoLocationFile
            // 
            this.label_GeoLocationFile.AutoSize = true;
            this.label_GeoLocationFile.Location = new System.Drawing.Point(7, 172);
            this.label_GeoLocationFile.Name = "label_GeoLocationFile";
            this.label_GeoLocationFile.Size = new System.Drawing.Size(90, 13);
            this.label_GeoLocationFile.TabIndex = 20;
            this.label_GeoLocationFile.Text = "Geo Location File";
            // 
            // comboBox_All
            // 
            this.comboBox_All.FormattingEnabled = true;
            this.comboBox_All.Items.AddRange(new object[] {
            "Park",
            "Parking Spot"});
            this.comboBox_All.Location = new System.Drawing.Point(15, 27);
            this.comboBox_All.Name = "comboBox_All";
            this.comboBox_All.Size = new System.Drawing.Size(118, 21);
            this.comboBox_All.TabIndex = 21;
            // 
            // btn_GetByIDorByName
            // 
            this.btn_GetByIDorByName.Location = new System.Drawing.Point(498, 534);
            this.btn_GetByIDorByName.Name = "btn_GetByIDorByName";
            this.btn_GetByIDorByName.Size = new System.Drawing.Size(148, 23);
            this.btn_GetByIDorByName.TabIndex = 22;
            this.btn_GetByIDorByName.Text = "Get Park/Parking Spot";
            this.btn_GetByIDorByName.UseVisualStyleBackColor = true;
            this.btn_GetByIDorByName.Click += new System.EventHandler(this.btn_GetByIDOrName_Click);
            // 
            // text_ByID
            // 
            this.text_ByID.Location = new System.Drawing.Point(381, 494);
            this.text_ByID.Name = "text_ByID";
            this.text_ByID.Size = new System.Drawing.Size(100, 20);
            this.text_ByID.TabIndex = 23;
            // 
            // label_ByID
            // 
            this.label_ByID.AutoSize = true;
            this.label_ByID.Location = new System.Drawing.Point(343, 497);
            this.label_ByID.Name = "label_ByID";
            this.label_ByID.Size = new System.Drawing.Size(18, 13);
            this.label_ByID.TabIndex = 24;
            this.label_ByID.Text = "ID";
            // 
            // list_ByID
            // 
            this.list_ByID.FormattingEnabled = true;
            this.list_ByID.HorizontalScrollbar = true;
            this.list_ByID.Location = new System.Drawing.Point(498, 420);
            this.list_ByID.Name = "list_ByID";
            this.list_ByID.Size = new System.Drawing.Size(290, 108);
            this.list_ByID.TabIndex = 25;
            // 
            // comboBox_NameOrId
            // 
            this.comboBox_NameOrId.FormattingEnabled = true;
            this.comboBox_NameOrId.Items.AddRange(new object[] {
            "Name",
            "Id"});
            this.comboBox_NameOrId.Location = new System.Drawing.Point(346, 456);
            this.comboBox_NameOrId.Name = "comboBox_NameOrId";
            this.comboBox_NameOrId.Size = new System.Drawing.Size(135, 21);
            this.comboBox_NameOrId.TabIndex = 26;
            // 
            // label_SpotBatteryStatus
            // 
            this.label_SpotBatteryStatus.AutoSize = true;
            this.label_SpotBatteryStatus.Location = new System.Drawing.Point(6, 174);
            this.label_SpotBatteryStatus.Name = "label_SpotBatteryStatus";
            this.label_SpotBatteryStatus.Size = new System.Drawing.Size(73, 13);
            this.label_SpotBatteryStatus.TabIndex = 41;
            this.label_SpotBatteryStatus.Text = "Battery Status";
            // 
            // label_SpotTimestamp
            // 
            this.label_SpotTimestamp.AutoSize = true;
            this.label_SpotTimestamp.Location = new System.Drawing.Point(6, 149);
            this.label_SpotTimestamp.Name = "label_SpotTimestamp";
            this.label_SpotTimestamp.Size = new System.Drawing.Size(58, 13);
            this.label_SpotTimestamp.TabIndex = 40;
            this.label_SpotTimestamp.Text = "Timestamp";
            // 
            // label_SpotStatus
            // 
            this.label_SpotStatus.AutoSize = true;
            this.label_SpotStatus.Location = new System.Drawing.Point(6, 120);
            this.label_SpotStatus.Name = "label_SpotStatus";
            this.label_SpotStatus.Size = new System.Drawing.Size(37, 13);
            this.label_SpotStatus.TabIndex = 39;
            this.label_SpotStatus.Text = "Status";
            // 
            // label_SpotType
            // 
            this.label_SpotType.AutoSize = true;
            this.label_SpotType.Location = new System.Drawing.Point(6, 94);
            this.label_SpotType.Name = "label_SpotType";
            this.label_SpotType.Size = new System.Drawing.Size(31, 13);
            this.label_SpotType.TabIndex = 38;
            this.label_SpotType.Text = "Type";
            // 
            // label_SpotSpotID
            // 
            this.label_SpotSpotID.AutoSize = true;
            this.label_SpotSpotID.Location = new System.Drawing.Point(6, 68);
            this.label_SpotSpotID.Name = "label_SpotSpotID";
            this.label_SpotSpotID.Size = new System.Drawing.Size(43, 13);
            this.label_SpotSpotID.TabIndex = 37;
            this.label_SpotSpotID.Text = "Spot ID";
            // 
            // label_SpotParkID
            // 
            this.label_SpotParkID.AutoSize = true;
            this.label_SpotParkID.Location = new System.Drawing.Point(6, 42);
            this.label_SpotParkID.Name = "label_SpotParkID";
            this.label_SpotParkID.Size = new System.Drawing.Size(43, 13);
            this.label_SpotParkID.TabIndex = 36;
            this.label_SpotParkID.Text = "Park ID";
            // 
            // label_SpotID
            // 
            this.label_SpotID.AutoSize = true;
            this.label_SpotID.Location = new System.Drawing.Point(6, 16);
            this.label_SpotID.Name = "label_SpotID";
            this.label_SpotID.Size = new System.Drawing.Size(18, 13);
            this.label_SpotID.TabIndex = 35;
            this.label_SpotID.Text = "ID";
            // 
            // text_SpotSpotID
            // 
            this.text_SpotSpotID.Location = new System.Drawing.Point(89, 65);
            this.text_SpotSpotID.Name = "text_SpotSpotID";
            this.text_SpotSpotID.Size = new System.Drawing.Size(129, 20);
            this.text_SpotSpotID.TabIndex = 30;
            // 
            // text_SpotID
            // 
            this.text_SpotID.Location = new System.Drawing.Point(89, 13);
            this.text_SpotID.Name = "text_SpotID";
            this.text_SpotID.Size = new System.Drawing.Size(129, 20);
            this.text_SpotID.TabIndex = 28;
            // 
            // text_SpotLocation
            // 
            this.text_SpotLocation.Location = new System.Drawing.Point(89, 195);
            this.text_SpotLocation.Name = "text_SpotLocation";
            this.text_SpotLocation.Size = new System.Drawing.Size(129, 20);
            this.text_SpotLocation.TabIndex = 43;
            // 
            // label_SpotLocation
            // 
            this.label_SpotLocation.AutoSize = true;
            this.label_SpotLocation.Location = new System.Drawing.Point(6, 198);
            this.label_SpotLocation.Name = "label_SpotLocation";
            this.label_SpotLocation.Size = new System.Drawing.Size(48, 13);
            this.label_SpotLocation.TabIndex = 44;
            this.label_SpotLocation.Text = "Location";
            // 
            // groupBox_Parks
            // 
            this.groupBox_Parks.Controls.Add(this.numericUpDown_ParkNumberOfSpecialSpots);
            this.groupBox_Parks.Controls.Add(this.btn_ParkDELETE);
            this.groupBox_Parks.Controls.Add(this.numericUpDown_ParkNumberOfSpots);
            this.groupBox_Parks.Controls.Add(this.btn_ParkPUT);
            this.groupBox_Parks.Controls.Add(this.btn_ParkPOST);
            this.groupBox_Parks.Controls.Add(this.label_ParkDescription);
            this.groupBox_Parks.Controls.Add(this.text_ParkID);
            this.groupBox_Parks.Controls.Add(this.text_ParkName);
            this.groupBox_Parks.Controls.Add(this.text_ParkDescription);
            this.groupBox_Parks.Controls.Add(this.text_ParkOperatingHours);
            this.groupBox_Parks.Controls.Add(this.text_ParkGeoLocationFile);
            this.groupBox_Parks.Controls.Add(this.label_ParkID);
            this.groupBox_Parks.Controls.Add(this.label_ParkName);
            this.groupBox_Parks.Controls.Add(this.label_NumberOfSpots);
            this.groupBox_Parks.Controls.Add(this.label_OperatingHours);
            this.groupBox_Parks.Controls.Add(this.label_NumberOfSpecialSpots);
            this.groupBox_Parks.Controls.Add(this.label_GeoLocationFile);
            this.groupBox_Parks.Location = new System.Drawing.Point(316, 80);
            this.groupBox_Parks.Name = "groupBox_Parks";
            this.groupBox_Parks.Size = new System.Drawing.Size(241, 309);
            this.groupBox_Parks.TabIndex = 45;
            this.groupBox_Parks.TabStop = false;
            this.groupBox_Parks.Text = "Parks";
            // 
            // numericUpDown_ParkNumberOfSpecialSpots
            // 
            this.numericUpDown_ParkNumberOfSpecialSpots.Location = new System.Drawing.Point(137, 144);
            this.numericUpDown_ParkNumberOfSpecialSpots.Name = "numericUpDown_ParkNumberOfSpecialSpots";
            this.numericUpDown_ParkNumberOfSpecialSpots.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown_ParkNumberOfSpecialSpots.TabIndex = 49;
            // 
            // btn_ParkDELETE
            // 
            this.btn_ParkDELETE.Location = new System.Drawing.Point(9, 279);
            this.btn_ParkDELETE.Name = "btn_ParkDELETE";
            this.btn_ParkDELETE.Size = new System.Drawing.Size(227, 23);
            this.btn_ParkDELETE.TabIndex = 49;
            this.btn_ParkDELETE.Text = "Delete";
            this.btn_ParkDELETE.UseVisualStyleBackColor = true;
            this.btn_ParkDELETE.Click += new System.EventHandler(this.btn_ParkDELETE_Click);
            // 
            // numericUpDown_ParkNumberOfSpots
            // 
            this.numericUpDown_ParkNumberOfSpots.Location = new System.Drawing.Point(137, 92);
            this.numericUpDown_ParkNumberOfSpots.Name = "numericUpDown_ParkNumberOfSpots";
            this.numericUpDown_ParkNumberOfSpots.Size = new System.Drawing.Size(100, 20);
            this.numericUpDown_ParkNumberOfSpots.TabIndex = 48;
            // 
            // btn_ParkPUT
            // 
            this.btn_ParkPUT.Location = new System.Drawing.Point(9, 250);
            this.btn_ParkPUT.Name = "btn_ParkPUT";
            this.btn_ParkPUT.Size = new System.Drawing.Size(227, 23);
            this.btn_ParkPUT.TabIndex = 48;
            this.btn_ParkPUT.Text = "Edit";
            this.btn_ParkPUT.UseVisualStyleBackColor = true;
            this.btn_ParkPUT.Click += new System.EventHandler(this.btn_ParkPUT_Click);
            // 
            // btn_ParkPOST
            // 
            this.btn_ParkPOST.Location = new System.Drawing.Point(10, 221);
            this.btn_ParkPOST.Name = "btn_ParkPOST";
            this.btn_ParkPOST.Size = new System.Drawing.Size(227, 23);
            this.btn_ParkPOST.TabIndex = 47;
            this.btn_ParkPOST.Text = "Create";
            this.btn_ParkPOST.UseVisualStyleBackColor = true;
            this.btn_ParkPOST.Click += new System.EventHandler(this.btn_ParkPOST_Click);
            // 
            // groupBox_ParkingSpots
            // 
            this.groupBox_ParkingSpots.Controls.Add(this.textBox_SpotParkID);
            this.groupBox_ParkingSpots.Controls.Add(this.comboBox_SpotType);
            this.groupBox_ParkingSpots.Controls.Add(this.btn_SpotDELETE);
            this.groupBox_ParkingSpots.Controls.Add(this.btn_SpotPUT);
            this.groupBox_ParkingSpots.Controls.Add(this.btn_SpotPOST);
            this.groupBox_ParkingSpots.Controls.Add(this.comboBox_SpotBatteryStatus);
            this.groupBox_ParkingSpots.Controls.Add(this.comboBox_SpotStatus);
            this.groupBox_ParkingSpots.Controls.Add(this.dateTimePicker_SpotDateTime);
            this.groupBox_ParkingSpots.Controls.Add(this.text_SpotID);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotLocation);
            this.groupBox_ParkingSpots.Controls.Add(this.text_SpotSpotID);
            this.groupBox_ParkingSpots.Controls.Add(this.text_SpotLocation);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotBatteryStatus);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotTimestamp);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotStatus);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotID);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotType);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotParkID);
            this.groupBox_ParkingSpots.Controls.Add(this.label_SpotSpotID);
            this.groupBox_ParkingSpots.Location = new System.Drawing.Point(563, 80);
            this.groupBox_ParkingSpots.Name = "groupBox_ParkingSpots";
            this.groupBox_ParkingSpots.Size = new System.Drawing.Size(224, 309);
            this.groupBox_ParkingSpots.TabIndex = 46;
            this.groupBox_ParkingSpots.TabStop = false;
            this.groupBox_ParkingSpots.Text = "Parking Spots";
            // 
            // textBox_SpotParkID
            // 
            this.textBox_SpotParkID.Location = new System.Drawing.Point(89, 39);
            this.textBox_SpotParkID.Name = "textBox_SpotParkID";
            this.textBox_SpotParkID.Size = new System.Drawing.Size(129, 20);
            this.textBox_SpotParkID.TabIndex = 47;
            // 
            // comboBox_SpotType
            // 
            this.comboBox_SpotType.FormattingEnabled = true;
            this.comboBox_SpotType.Items.AddRange(new object[] {
            "ParkingSpot",
            "SpecialSpot"});
            this.comboBox_SpotType.Location = new System.Drawing.Point(89, 91);
            this.comboBox_SpotType.Name = "comboBox_SpotType";
            this.comboBox_SpotType.Size = new System.Drawing.Size(129, 21);
            this.comboBox_SpotType.TabIndex = 47;
            // 
            // btn_SpotDELETE
            // 
            this.btn_SpotDELETE.Location = new System.Drawing.Point(9, 279);
            this.btn_SpotDELETE.Name = "btn_SpotDELETE";
            this.btn_SpotDELETE.Size = new System.Drawing.Size(209, 23);
            this.btn_SpotDELETE.TabIndex = 50;
            this.btn_SpotDELETE.Text = "Delete";
            this.btn_SpotDELETE.UseVisualStyleBackColor = true;
            this.btn_SpotDELETE.Click += new System.EventHandler(this.btn_SpotDELETE_Click);
            // 
            // btn_SpotPUT
            // 
            this.btn_SpotPUT.Location = new System.Drawing.Point(9, 250);
            this.btn_SpotPUT.Name = "btn_SpotPUT";
            this.btn_SpotPUT.Size = new System.Drawing.Size(209, 23);
            this.btn_SpotPUT.TabIndex = 49;
            this.btn_SpotPUT.Text = "Edit";
            this.btn_SpotPUT.UseVisualStyleBackColor = true;
            this.btn_SpotPUT.Click += new System.EventHandler(this.btn_SpotPUT_Click);
            // 
            // btn_SpotPOST
            // 
            this.btn_SpotPOST.Location = new System.Drawing.Point(9, 221);
            this.btn_SpotPOST.Name = "btn_SpotPOST";
            this.btn_SpotPOST.Size = new System.Drawing.Size(209, 23);
            this.btn_SpotPOST.TabIndex = 48;
            this.btn_SpotPOST.Text = "Create";
            this.btn_SpotPOST.UseVisualStyleBackColor = true;
            this.btn_SpotPOST.Click += new System.EventHandler(this.btn_SpotPOST_Click);
            // 
            // comboBox_SpotBatteryStatus
            // 
            this.comboBox_SpotBatteryStatus.FormattingEnabled = true;
            this.comboBox_SpotBatteryStatus.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_SpotBatteryStatus.Location = new System.Drawing.Point(89, 171);
            this.comboBox_SpotBatteryStatus.Name = "comboBox_SpotBatteryStatus";
            this.comboBox_SpotBatteryStatus.Size = new System.Drawing.Size(129, 21);
            this.comboBox_SpotBatteryStatus.TabIndex = 47;
            // 
            // comboBox_SpotStatus
            // 
            this.comboBox_SpotStatus.FormattingEnabled = true;
            this.comboBox_SpotStatus.Items.AddRange(new object[] {
            "True",
            "False"});
            this.comboBox_SpotStatus.Location = new System.Drawing.Point(89, 117);
            this.comboBox_SpotStatus.Name = "comboBox_SpotStatus";
            this.comboBox_SpotStatus.Size = new System.Drawing.Size(129, 21);
            this.comboBox_SpotStatus.TabIndex = 47;
            // 
            // dateTimePicker_SpotDateTime
            // 
            this.dateTimePicker_SpotDateTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            this.dateTimePicker_SpotDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_SpotDateTime.Location = new System.Drawing.Point(89, 145);
            this.dateTimePicker_SpotDateTime.Name = "dateTimePicker_SpotDateTime";
            this.dateTimePicker_SpotDateTime.ShowUpDown = true;
            this.dateTimePicker_SpotDateTime.Size = new System.Drawing.Size(129, 20);
            this.dateTimePicker_SpotDateTime.TabIndex = 47;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(343, 523);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Name";
            // 
            // text_ByName
            // 
            this.text_ByName.Location = new System.Drawing.Point(381, 520);
            this.text_ByName.Name = "text_ByName";
            this.text_ByName.Size = new System.Drawing.Size(100, 20);
            this.text_ByName.TabIndex = 47;
            // 
            // dateTime_SpecificMoment
            // 
            this.dateTime_SpecificMoment.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            this.dateTime_SpecificMoment.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTime_SpecificMoment.Location = new System.Drawing.Point(54, 229);
            this.dateTime_SpecificMoment.Name = "dateTime_SpecificMoment";
            this.dateTime_SpecificMoment.ShowUpDown = true;
            this.dateTime_SpecificMoment.Size = new System.Drawing.Size(129, 20);
            this.dateTime_SpecificMoment.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Specific Moment:";
            // 
            // dateTime_Start
            // 
            this.dateTime_Start.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            this.dateTime_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTime_Start.Location = new System.Drawing.Point(58, 341);
            this.dateTime_Start.Name = "dateTime_Start";
            this.dateTime_Start.ShowUpDown = true;
            this.dateTime_Start.Size = new System.Drawing.Size(129, 20);
            this.dateTime_Start.TabIndex = 52;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Start";
            // 
            // dateTime_End
            // 
            this.dateTime_End.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            this.dateTime_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTime_End.Location = new System.Drawing.Point(58, 376);
            this.dateTime_End.Name = "dateTime_End";
            this.dateTime_End.ShowUpDown = true;
            this.dateTime_End.Size = new System.Drawing.Size(129, 20);
            this.dateTime_End.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 379);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "End";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(16, 310);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "SpecificTime Period:";
            // 
            // btn_GetParkingSpotsSpecificMoment
            // 
            this.btn_GetParkingSpotsSpecificMoment.Location = new System.Drawing.Point(15, 264);
            this.btn_GetParkingSpotsSpecificMoment.Name = "btn_GetParkingSpotsSpecificMoment";
            this.btn_GetParkingSpotsSpecificMoment.Size = new System.Drawing.Size(118, 23);
            this.btn_GetParkingSpotsSpecificMoment.TabIndex = 57;
            this.btn_GetParkingSpotsSpecificMoment.Text = "Get Parking Spots";
            this.btn_GetParkingSpotsSpecificMoment.UseVisualStyleBackColor = true;
            this.btn_GetParkingSpotsSpecificMoment.Click += new System.EventHandler(this.btn_GetParkingSpotsSpecificMoment_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 61;
            this.label6.Text = "Name";
            // 
            // text_ParkByName
            // 
            this.text_ParkByName.Location = new System.Drawing.Point(54, 162);
            this.text_ParkByName.Name = "text_ParkByName";
            this.text_ParkByName.Size = new System.Drawing.Size(129, 20);
            this.text_ParkByName.TabIndex = 60;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 13);
            this.label7.TabIndex = 59;
            this.label7.Text = "ID";
            // 
            // text_ParkByID
            // 
            this.text_ParkByID.Location = new System.Drawing.Point(54, 136);
            this.text_ParkByID.Name = "text_ParkByID";
            this.text_ParkByID.Size = new System.Drawing.Size(129, 20);
            this.text_ParkByID.TabIndex = 58;
            // 
            // btn_GetParkingSpotsSpecificPark
            // 
            this.btn_GetParkingSpotsSpecificPark.Location = new System.Drawing.Point(142, 97);
            this.btn_GetParkingSpotsSpecificPark.Name = "btn_GetParkingSpotsSpecificPark";
            this.btn_GetParkingSpotsSpecificPark.Size = new System.Drawing.Size(160, 23);
            this.btn_GetParkingSpotsSpecificPark.TabIndex = 63;
            this.btn_GetParkingSpotsSpecificPark.Text = "Get Parking Spots From Park";
            this.btn_GetParkingSpotsSpecificPark.UseVisualStyleBackColor = true;
            this.btn_GetParkingSpotsSpecificPark.Click += new System.EventHandler(this.btn_GetParkingSpotsSpecificPark_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 13);
            this.label8.TabIndex = 62;
            this.label8.Text = "Specific Park:";
            // 
            // btn_GetParkingSpotsFromParkSpecificMoment
            // 
            this.btn_GetParkingSpotsFromParkSpecificMoment.Location = new System.Drawing.Point(142, 264);
            this.btn_GetParkingSpotsFromParkSpecificMoment.Name = "btn_GetParkingSpotsFromParkSpecificMoment";
            this.btn_GetParkingSpotsFromParkSpecificMoment.Size = new System.Drawing.Size(160, 23);
            this.btn_GetParkingSpotsFromParkSpecificMoment.TabIndex = 64;
            this.btn_GetParkingSpotsFromParkSpecificMoment.Text = "Get Parking Spots From Park";
            this.btn_GetParkingSpotsFromParkSpecificMoment.UseVisualStyleBackColor = true;
            this.btn_GetParkingSpotsFromParkSpecificMoment.Click += new System.EventHandler(this.btn_GetParkingSpotsFromParkSpecificMoment_Click);
            // 
            // btn_GetParkingSpotsFromParkSpecificTimePeriod
            // 
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.Location = new System.Drawing.Point(145, 412);
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.Name = "btn_GetParkingSpotsFromParkSpecificTimePeriod";
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.Size = new System.Drawing.Size(160, 23);
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.TabIndex = 65;
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.Text = "Get Parking Spots From Park";
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.UseVisualStyleBackColor = true;
            this.btn_GetParkingSpotsFromParkSpecificTimePeriod.Click += new System.EventHandler(this.btn_GetParkingSpotsFromParkSpecificTimePeriod_Click);
            // 
            // list_All
            // 
            this.list_All.FormattingEnabled = true;
            this.list_All.HorizontalScrollbar = true;
            this.list_All.Location = new System.Drawing.Point(12, 450);
            this.list_All.Name = "list_All";
            this.list_All.Size = new System.Drawing.Size(290, 134);
            this.list_All.TabIndex = 1;
            // 
            // comboBox_selectType
            // 
            this.comboBox_selectType.FormattingEnabled = true;
            this.comboBox_selectType.Items.AddRange(new object[] {
            "Park",
            "Parking Spot"});
            this.comboBox_selectType.Location = new System.Drawing.Point(346, 420);
            this.comboBox_selectType.Name = "comboBox_selectType";
            this.comboBox_selectType.Size = new System.Drawing.Size(135, 21);
            this.comboBox_selectType.TabIndex = 66;
            // 
            // comboBox_NameOrIdByPark
            // 
            this.comboBox_NameOrIdByPark.FormattingEnabled = true;
            this.comboBox_NameOrIdByPark.Items.AddRange(new object[] {
            "Name",
            "Id"});
            this.comboBox_NameOrIdByPark.Location = new System.Drawing.Point(15, 99);
            this.comboBox_NameOrIdByPark.Name = "comboBox_NameOrIdByPark";
            this.comboBox_NameOrIdByPark.Size = new System.Drawing.Size(118, 21);
            this.comboBox_NameOrIdByPark.TabIndex = 67;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(322, 43);
            this.label9.MaximumSize = new System.Drawing.Size(400, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(395, 26);
            this.label9.TabIndex = 68;
            this.label9.Text = "Please note: All fields are required in case of Edit or Delete operations. If you" +
    " want to create either a Park or a Parking Spot, you can leave the field \"ID\" em" +
    "pty.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 596);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox_NameOrIdByPark);
            this.Controls.Add(this.comboBox_selectType);
            this.Controls.Add(this.btn_GetParkingSpotsFromParkSpecificTimePeriod);
            this.Controls.Add(this.btn_GetParkingSpotsFromParkSpecificMoment);
            this.Controls.Add(this.btn_GetParkingSpotsSpecificPark);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.text_ParkByName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.text_ParkByID);
            this.Controls.Add(this.btn_GetParkingSpotsSpecificMoment);
            this.Controls.Add(btn_GetParkingSpotsSpecificTimePeriod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTime_End);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTime_Start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTime_SpecificMoment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_ByName);
            this.Controls.Add(this.groupBox_ParkingSpots);
            this.Controls.Add(this.groupBox_Parks);
            this.Controls.Add(this.comboBox_NameOrId);
            this.Controls.Add(this.list_ByID);
            this.Controls.Add(this.label_ByID);
            this.Controls.Add(this.text_ByID);
            this.Controls.Add(this.btn_GetByIDorByName);
            this.Controls.Add(this.comboBox_All);
            this.Controls.Add(this.list_All);
            this.Controls.Add(this.btn_GetAllParksOrParkingSpots);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox_Parks.ResumeLayout(false);
            this.groupBox_Parks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParkNumberOfSpecialSpots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParkNumberOfSpots)).EndInit();
            this.groupBox_ParkingSpots.ResumeLayout(false);
            this.groupBox_ParkingSpots.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_GetAllParksOrParkingSpots;
        private System.Windows.Forms.TextBox text_ParkID;
        private System.Windows.Forms.TextBox text_ParkName;
        private System.Windows.Forms.TextBox text_ParkDescription;
        private System.Windows.Forms.TextBox text_ParkOperatingHours;
        private System.Windows.Forms.TextBox text_ParkGeoLocationFile;
        private System.Windows.Forms.Label label_ParkID;
        private System.Windows.Forms.Label label_ParkName;
        private System.Windows.Forms.Label label_ParkDescription;
        private System.Windows.Forms.Label label_NumberOfSpots;
        private System.Windows.Forms.Label label_OperatingHours;
        private System.Windows.Forms.Label label_NumberOfSpecialSpots;
        private System.Windows.Forms.Label label_GeoLocationFile;
        private System.Windows.Forms.ComboBox comboBox_All;
        private System.Windows.Forms.Button btn_GetByIDorByName;
        private System.Windows.Forms.TextBox text_ByID;
        private System.Windows.Forms.Label label_ByID;
        private System.Windows.Forms.ListBox list_ByID;
        private System.Windows.Forms.ComboBox comboBox_NameOrId;
        private System.Windows.Forms.Label label_SpotBatteryStatus;
        private System.Windows.Forms.Label label_SpotTimestamp;
        private System.Windows.Forms.Label label_SpotStatus;
        private System.Windows.Forms.Label label_SpotType;
        private System.Windows.Forms.Label label_SpotSpotID;
        private System.Windows.Forms.Label label_SpotParkID;
        private System.Windows.Forms.Label label_SpotID;
        private System.Windows.Forms.TextBox text_SpotSpotID;
        private System.Windows.Forms.TextBox text_SpotID;
        private System.Windows.Forms.TextBox text_SpotLocation;
        private System.Windows.Forms.Label label_SpotLocation;
        private System.Windows.Forms.GroupBox groupBox_Parks;
        private System.Windows.Forms.GroupBox groupBox_ParkingSpots;
        private System.Windows.Forms.DateTimePicker dateTimePicker_SpotDateTime;
        private System.Windows.Forms.ComboBox comboBox_SpotBatteryStatus;
        private System.Windows.Forms.ComboBox comboBox_SpotStatus;
        private System.Windows.Forms.Button btn_ParkDELETE;
        private System.Windows.Forms.Button btn_ParkPUT;
        private System.Windows.Forms.Button btn_ParkPOST;
        private System.Windows.Forms.Button btn_SpotDELETE;
        private System.Windows.Forms.Button btn_SpotPUT;
        private System.Windows.Forms.Button btn_SpotPOST;
        private System.Windows.Forms.ComboBox comboBox_SpotType;
        private System.Windows.Forms.NumericUpDown numericUpDown_ParkNumberOfSpots;
        private System.Windows.Forms.NumericUpDown numericUpDown_ParkNumberOfSpecialSpots;
        private System.Windows.Forms.TextBox textBox_SpotParkID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox text_ByName;
        private System.Windows.Forms.DateTimePicker dateTime_SpecificMoment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTime_Start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTime_End;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_GetParkingSpotsSpecificMoment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox text_ParkByName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox text_ParkByID;
        private System.Windows.Forms.Button btn_GetParkingSpotsSpecificPark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_GetParkingSpotsFromParkSpecificMoment;
        private System.Windows.Forms.Button btn_GetParkingSpotsFromParkSpecificTimePeriod;
        private System.Windows.Forms.ListBox list_All;
        private System.Windows.Forms.ComboBox comboBox_selectType;
        private System.Windows.Forms.ComboBox comboBox_NameOrIdByPark;
        private System.Windows.Forms.Label label9;
    }
}

