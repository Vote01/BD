using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Common;
using System.Drawing.Imaging;
using Microsoft.Data.Sqlite;
using System.Collections;



namespace kursach
{
    public partial class Form1 : Form
    {
        private sqliteclass mydb = null;
        private string sCurDir = string.Empty;
        private string sPath = string.Empty;
        private string sSql = string.Empty;
        public byte[] data = null;



        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;


        }



        private void button1_Click(object sender, EventArgs e)
        {

            mydb = new sqliteclass();

            int index = comboBox1.SelectedIndex;

            switch (index)
            {
                case 0:
                    sSql = @"insert into [Place rental] (DATA_START, DATA_FINISH, PRICE_PLACE,DATA_RENT, NAME_PLACE, AREA_PLACE, FIO_TENANT) 
                    values ('" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "','" + dataGridView2[1, 3].Value
                     + "','" + dataGridView2[1, 4].Value + "','" + dataGridView2[1, 5].Value + "','" + dataGridView2[1, 6].Value + "','" + dataGridView2[1, 7].Value + "');";

                    break;
                case 1:
                    sSql = @"insert into [Warehouse rental] (DATA_START, DATA_FINISH, PRICE_WARE, DATA_RENT, AREA_WARE, WARE_ID, FIO_TENANT) 
                    values ('" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "','" + dataGridView2[1, 3].Value
                    + "','" + dataGridView2[1, 4].Value + "','" + dataGridView2[1, 5].Value + "','" + dataGridView2[1, 6].Value + "','" + dataGridView2[1, 7].Value + "');";
                    break;
                case 2:
                    sSql = @"insert into [Equipment rental] (DATA_START, DATA_FINISH, PRICE_EQUI, DATA_RENT, NAME_EQUI, COUNT, FIO_TENANT) 
                    values ('" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "','" + dataGridView2[1, 3].Value
                    + "','" + dataGridView2[1, 4].Value + "','" + dataGridView2[1, 5].Value + "','" + dataGridView2[1, 6].Value + "','" + dataGridView2[1, 7].Value + "');";
                    break;
                case 3:

                    sSql = @"insert into [Employee] (FIO_EMPL, POST, SER_PAS, NUMB_PAS, ADDRESS, NAME_PLACE) 
                    values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "','" + dataGridView2[1, 3].Value
                      + "','" + dataGridView2[1, 4].Value + "','" + dataGridView2[1, 6].Value + "');";
                    break;
                case 4:
                    sSql = @"insert into [Tenant](FIO_TENANT) values ('" + dataGridView2[1, 0].Value + "');";
                    break;
                case 5:
                    sSql = @"insert into [Warehouse](WARE_ID,AREA_WARE,PRICE_WARE) values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "');";
                    break;
                case 6:
                    sSql = @"insert into [Place](NAME_PLACE,AREA_PLACE,PRICE_PLACE) values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "');";
                    break;
                case 7:
                    sSql = @"insert into [Equipment](NAME_EQUI,COUNT_EQUI,PRICE_EQUI) values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "');";
                    break;
                case 8:
                    sSql = @"insert into [Sancontrol](DATA_SAN, RESULT,PLACE) values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 2].Value + "','" + dataGridView2[1, 3].Value + "');";
                    break;
                case 9:
                    sSql = @"insert into [Product](NAME_PROD,COUNT_PROD,WARE_ID) values ('" + dataGridView2[1, 0].Value + "','" + dataGridView2[1, 1].Value + "','" + dataGridView2[1, 2].Value + "');";
                    break;
            }

            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                MessageBox.Show("Ошибка добавления записи!");
            }
            else MessageBox.Show("Запись добавлена!");
            mydb = null;

            return;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sPath = Path.Combine(Application.StartupPath, "kurs.db");

        }



        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;

            mydb = new sqliteclass();
            DataRow[] datarows = null;
            DataRow[] datarows2 = null;
            DataRow[] datarows3 = null;
            numericUpDown2.Visible = false;
            label6.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            label4.Text = "";
            dataGridView3.ColumnHeadersVisible = true;
            ArrayList arrayList = new ArrayList();
            ArrayList array2 = new ArrayList();
            ArrayList array3 = new ArrayList();
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды помещения", "Дата заключения договора", "Название помещения", "Площадь помещения", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;

                    sSql = "select * from [Place rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_PLACE"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["NAME_PLACE"], dr["AREA_PLACE"], dr["FIO_TENANT"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 1:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды склада", "Дата заключения договора", "Площадь склада", "Номер склада", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;

                    sSql = "select * from [Warehouse rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_WARE"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["AREA_WARE"], dr["WARE_ID"], dr["FIO_TENANT"]);

                    sSql = "select * from [Warehouse]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Склады:";
                    array3.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 2:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды оборудования", "Дата заключения договора", "Название инструмента", "Количество", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;
                    numericUpDown2.Visible = true;
                    label6.Visible = true;
                    sSql = "select * from [Equipment rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_EQUI"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["NAME_EQUI"], dr["COUNT"], dr["FIO_TENANT"]);

                    sSql = "select * from [Equipment]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Оборудование:";
                    array3.AddRange(new string[] { "Название оборудования", "Количество", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_EQUI"], dr["COUNT_EQUI"], dr["PRICE_EQUI"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 3:
                    arrayList.AddRange(new string[] { "ФИО сотрудника", "Должность", "Серия паспорта", "Номер паспорта", "Адрес", "Фото", "Название помещения" });
                    dataGridView1.ColumnCount = 7;

                    sSql = "select * from Employee";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["FIO_EMPL"], dr["POST"], dr["SER_PAS"], dr["NUMB_PAS"], dr["ADDRESS"], dr["PHOTO"], dr["NAME_PLACE"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    break;
                case 4:
                    arrayList.AddRange(new string[] { "ФИО арендатора" });
                    dataGridView1.ColumnCount = 1;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Tenant";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["FIO_TENANT"]);
                    break;
                case 5:
                    arrayList.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Warehouse";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);
                    break;
                case 6:
                    arrayList.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Place";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);
                    break;
                case 7:
                    arrayList.AddRange(new string[] { "Название оборудования", "Количество", "Цена" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Equipment";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_EQUI"], dr["COUNT_EQUI"], dr["PRICE_EQUI"]);
                    break;
                case 8:
                    arrayList.AddRange(new string[] { "Дата санконтроля", "Номер санконтроля", "Результат", "Помещение" });
                    dataGridView1.ColumnCount = 4;
                    sSql = "select * from Sancontrol";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(Convert.ToDateTime(dr["DATA_SAN"]).ToString("yyyy-MM-dd"), dr["ID_SAN"], dr["RESULT"], dr["PLACE"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    break;
                case 9:
                    arrayList.AddRange(new string[] { "Название продукта", "Количество", "Номер склада" });
                    dataGridView1.ColumnCount = 3;

                    sSql = "select * from Product";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_PROD"], dr["COUNT_PROD"], dr["WARE_ID"]);

                    sSql = "select * from [Warehouse]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Склады:";
                    array3.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);

                    break;
            }
            array2.AddRange(arrayList);
            for (int i = 0; i < array2.Count; i++)
                dataGridView1.Columns[i].HeaderText = array2[i].ToString();
            for (int i = 0; i < array3.Count; i++)
                dataGridView3.Columns[i].HeaderText = array3[i].ToString();
            foreach (var item in arrayList)
                dataGridView2.Rows.Add(item);
            mydb = null;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;

            mydb = new sqliteclass();
            DataRow[] datarows = null;
            DataRow[] datarows2 = null;
            DataRow[] datarows3 = null;
            numericUpDown2.Visible = false;
            label6.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            label4.Text = "";
            dataGridView3.ColumnHeadersVisible = true;
            ArrayList arrayList = new ArrayList();
            ArrayList array2 = new ArrayList();
            ArrayList array3 = new ArrayList();
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды помещения", "Дата заключения договора", "Название помещения", "Площадь помещения", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;

                    sSql = "select * from [Place rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_PLACE"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["NAME_PLACE"], dr["AREA_PLACE"], dr["FIO_TENANT"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 1:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды склада", "Дата заключения договора", "Площадь склада", "Номер склада", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;

                    sSql = "select * from [Warehouse rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_WARE"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["AREA_WARE"], dr["WARE_ID"], dr["FIO_TENANT"]);

                    sSql = "select * from [Warehouse]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Склады:";
                    array3.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 2:
                    arrayList.AddRange(new string[] { "Номер договора", "Дата начала договора", "Дата конца договора", "Цена аренды оборудования", "Дата заключения договора", "Название инструмента", "Количество", "ФИО арендатора" });
                    dataGridView1.ColumnCount = 8;
                    numericUpDown2.Visible = true;
                    label6.Visible = true;
                    sSql = "select * from [Equipment rental]";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["ID_RENT"], Convert.ToDateTime(dr["DATA_START"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["DATA_FINISH"]).ToString("yyyy-MM-dd"), dr["PRICE_EQUI"], Convert.ToDateTime(dr["DATA_RENT"]).ToString("yyyy-MM-dd"), dr["NAME_EQUI"], dr["COUNT"], dr["FIO_TENANT"]);

                    sSql = "select * from [Equipment]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Оборудование:";
                    array3.AddRange(new string[] { "Название оборудования", "Количество", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_EQUI"], dr["COUNT_EQUI"], dr["PRICE_EQUI"]);

                    sSql = "select * from Tenant";
                    datarows3 = mydb.drExecute(sPath, sSql);
                    dataGridView4.ColumnCount = 1;
                    foreach (DataRow dr in datarows3)
                        dataGridView4.Rows.Add(dr["FIO_TENANT"]);

                    break;
                case 3:
                    arrayList.AddRange(new string[] { "ФИО сотрудника", "Должность", "Серия паспорта", "Номер паспорта", "Адрес", "Фото", "Название помещения" });
                    dataGridView1.ColumnCount = 7;

                    sSql = "select * from Employee";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["FIO_EMPL"], dr["POST"], dr["SER_PAS"], dr["NUMB_PAS"], dr["ADDRESS"], dr["PHOTO"], dr["NAME_PLACE"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    break;
                case 4:
                    arrayList.AddRange(new string[] { "ФИО арендатора" });
                    dataGridView1.ColumnCount = 1;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Tenant";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["FIO_TENANT"]);
                    break;
                case 5:
                    arrayList.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Warehouse";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);
                    break;
                case 6:
                    arrayList.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Place";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);
                    break;
                case 7:
                    arrayList.AddRange(new string[] { "Название оборудования", "Количество", "Цена" });
                    dataGridView1.ColumnCount = 3;
                    dataGridView3.ColumnHeadersVisible = false;
                    sSql = "select * from Equipment";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_EQUI"], dr["COUNT_EQUI"], dr["PRICE_EQUI"]);
                    break;
                case 8:
                    arrayList.AddRange(new string[] { "Дата санконтроля", "Номер санконтроля", "Результат", "Помещение" });
                    dataGridView1.ColumnCount = 4;
                    sSql = "select * from Sancontrol";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(Convert.ToDateTime(dr["DATA_SAN"]).ToString("yyyy-MM-dd"), dr["ID_SAN"], dr["RESULT"], dr["PLACE"]);

                    sSql = "select * from [Place]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Площади:";
                    array3.AddRange(new string[] { "Название помещения", "Площадь", "Цена" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["NAME_PLACE"], dr["AREA_PLACE"], dr["PRICE_PLACE"]);

                    break;
                case 9:
                    arrayList.AddRange(new string[] { "Название продукта", "Количество", "Номер склада" });
                    dataGridView1.ColumnCount = 3;

                    sSql = "select * from Product";
                    datarows = mydb.drExecute(sPath, sSql);
                    foreach (DataRow dr in datarows)
                        dataGridView1.Rows.Add(dr["NAME_PROD"], dr["COUNT_PROD"], dr["WARE_ID"]);

                    sSql = "select * from [Warehouse]";
                    datarows2 = mydb.drExecute(sPath, sSql);
                    dataGridView3.ColumnCount = 3;
                    label4.Text = "Склады:";
                    array3.AddRange(new string[] { "Номер склада", "Площадь", "Цена аренды склада" });
                    foreach (DataRow dr in datarows2)
                        dataGridView3.Rows.Add(dr["WARE_ID"], dr["AREA_WARE"], dr["PRICE_WARE"]);

                    break;
            }
            array2.AddRange(arrayList);
            for (int i = 0; i < array2.Count; i++)
                dataGridView1.Columns[i].HeaderText = array2[i].ToString();
            for (int i = 0; i < array3.Count; i++)
                dataGridView3.Columns[i].HeaderText = array3[i].ToString();
            foreach (var item in arrayList)
                dataGridView2.Rows.Add(item);
            mydb = null;

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2[1, i].ReadOnly = false;
            }
            dataGridView2.ReadOnly = false;
            int index = comboBox1.SelectedIndex;
            int count = 0;

            switch (index)
            {
                case 0:
                    count = 8;
                    for (int i = 0; i < count; i++)
                    {
                        dataGridView2[1, i].ReadOnly = true;
                    }
                    
                    break;
                case 1:
                    count = 8;
                    for (int i = 0; i < count; i++)
                    {
                        dataGridView2[1, i].ReadOnly = true;
                    }
                  

                    break;
                case 2:
                    count = 8;
                    for (int i = 0; i < count; i++)
                    {
                        dataGridView2[1, i].ReadOnly = true;
                    }
                   

                    break;
                case 3:
                    count = 7;
                    try
                    {
                        mydb = new sqliteclass();

                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            data = row.Cells[5].Value == DBNull.Value ? null : (byte[])row.Cells[5].Value;
                            if (data != null)
                            {
                                MemoryStream ms = new MemoryStream(data);
                                pictureBox2.Image = Image.FromStream(ms);
                            }
                            else
                            {
                                pictureBox2.Image = null;
                            }
                            row.Cells[5].Value = " ";
                        }
                       

                        mydb = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Изображение не удалось загрузить!");
                    }

                    pictureBox2.Visible = true;

                    dataGridView2[1, 6].ReadOnly = true;
                    dataGridView2[1, 5].ReadOnly = true;

                    break;
                case 4:
                    count = 1;
                    break;
                case 5:
                    count = 3;
                    break;
                case 6:
                    count = 3;
                    break;
                case 7:
                    count = 3;
                    break;
                case 8:
                    count = 4;
                    dataGridView2[1, 3].ReadOnly = true;
                    break;
                case 9:
                    count = 3;
                    dataGridView2[1, 2].ReadOnly = true;
                    break;
            }
            for (int i = 0; i < count; i++)
            {
                dataGridView2[1, i].Value = dataGridView1[i, dataGridView1.CurrentCell.RowIndex].Value;
            }


        }



        private void button3_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            DataRow[] datarows = null;
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from [Place rental] where ID_RENT=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }

                    break;
                case 1:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from [Warehouse rental] where ID_RENT=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from [Equipment rental] where ID_RENT=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Employee where SER_PAS=('" + row.Cells[2].Value.ToString() + "') AND NUMB_PAS=('" + row.Cells[3].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 4:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Tenant where FIO_TENANT=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 5:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {

                        sSql = "delete from Warehouse where WARE_ID=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);

                    }

                    break;
                case 6:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Place where NAME_PLACE=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 7:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Equipment where NAME_EQUI=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 8:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Sancontrol where ID_SAN=('" + row.Cells[1].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
                case 9:
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        sSql = "delete from Product where NAME_PROD=('" + row.Cells[0].Value.ToString() + "');";
                        dataGridView1.Rows.Remove(row);
                        datarows = mydb.drExecute(sPath, sSql);
                    }
                    break;
            }
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
               // MessageBox.Show("Ошибка удаления записи!");
            }
            else MessageBox.Show("Запись удалена!");
            mydb = null;
            return;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            mydb = new sqliteclass();
            DataRow[] datarows = null;
            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 3:


                    if (data != null)
                    {
                        sSql = "update [Employee] set FIO_EMPL=('" + dataGridView2[1, 0].Value + "'), POST=('" + dataGridView2[1, 1].Value + "'),ADDRESS=('" + dataGridView2[1, 4].Value + "'), PHOTO=('" + data + "'), NAME_PLACE=('" + dataGridView2[1, 6].Value + "') where SER_PAS=('" + dataGridView2[1, 2].Value + "') and NUMB_PAS=('" + dataGridView2[1, 3].Value + "');";
                    }
                    else sSql = "update [Employee] set FIO_EMPL=('" + dataGridView2[1, 0].Value + "'), POST=('" + dataGridView2[1, 1].Value + "'),ADDRESS=('" + dataGridView2[1, 4].Value + "'), NAME_PLACE=('" + dataGridView2[1, 6].Value + "') where SER_PAS=('" + dataGridView2[1, 2].Value + "') and NUMB_PAS=('" + dataGridView2[1, 3].Value + "');";

                    datarows = mydb.drExecute(sPath, sSql);
                    break;
                case 5:
                    sSql = "update [Warehouse] set AREA_WARE=('" + dataGridView2[1, 1].Value + "'),PRICE_WARE=('" + dataGridView2[1, 2].Value + "') where WARE_ID=('" + dataGridView2[1, 0].Value + "');";
                    datarows = mydb.drExecute(sPath, sSql);
                    break;
                case 6:
                    sSql = "update [Place] set AREA_PLACE=('" + dataGridView2[1, 1].Value + "'),PRICE_PLACE=('" + dataGridView2[1, 2].Value + "') where NAME_PLACE=('" + dataGridView2[1, 0].Value + "');";
                    datarows = mydb.drExecute(sPath, sSql);
                    break;
                case 7:
                    sSql = "update [Equipment] set COUNT_EQUI=('" + dataGridView2[1, 1].Value + "'),PRICE_EQUI=('" + dataGridView2[1, 2].Value + "') where NAME_EQUI=('" + dataGridView2[1, 0].Value + "');";
                    datarows = mydb.drExecute(sPath, sSql);
                    break;
                case 9:
                    sSql = "update [Product] set COUNT_PROD=('" + dataGridView2[1, 1].Value + "') where NAME_PROD=('" + dataGridView2[1, 0].Value + "');";
                    datarows = mydb.drExecute(sPath, sSql);
                    break;
                default:
                    MessageBox.Show("Не подлежит изменению");
                    break;
            }
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                MessageBox.Show("Ошибка изменения записи!");
            }
            else MessageBox.Show("Запись изменена!");
            mydb = null;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "All Embroidery Files | *.bmp; *.gif; *.jpeg; *.jpg; " + "*.fif;*.fiff;*.png;*.wmf;*.emf" +
 "|Windows Bitmap (*.bmp)|*.bmp" + "|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg" + "|Graphics Interchange Format (*.gif)|*.gif" +
 "|Portable Network Graphics (*.png)|*.png" + "|Tag Embroidery File Format (*.tif)|*.tif;*.tiff";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image image = Image.FromFile(openFileDialog1.FileName);
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Jpeg);
                data = memoryStream.ToArray();
                pictureBox1.Image = image;

            }

        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            int index = comboBox1.SelectedIndex;
            switch (index)
            {
                case 0:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                           int PriceFin = (Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(numericUpDown1.Value));

                        dataGridView2[1, 3].Value = PriceFin;
                        dataGridView2[1, 5].Value = row.Cells[0].Value;
                        dataGridView2[1, 6].Value = row.Cells[1].Value;

                    }

                    break;
                case 1:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        int PriceFin = (Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(numericUpDown1.Value));
                        dataGridView2[1, 3].Value = PriceFin;
                        dataGridView2[1, 6].Value = row.Cells[0].Value;
                        dataGridView2[1, 5].Value = row.Cells[1].Value;
                    }
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        int Count = Convert.ToInt32(numericUpDown2.Value);
                        if (Count > Convert.ToInt32(row.Cells[1].Value)) { Count = Convert.ToInt32(row.Cells[1].Value);
                            numericUpDown2.Value = Count;
                        }
                        int PriceFin = (Convert.ToInt32(row.Cells[2].Value) * Convert.ToInt32(numericUpDown1.Value) * Count);
                        dataGridView2[1, 3].Value = PriceFin;
                        dataGridView2[1, 5].Value = row.Cells[0].Value;
                        dataGridView2[1, 6].Value = (int)numericUpDown2.Value;
                    }
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {

                        dataGridView2[1, 6].Value = row.Cells[0].Value;
                    }
                    break;

                case 8:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        dataGridView2[1, 3].Value = row.Cells[0].Value;

                    }
                    break;
                case 9:
                    foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                    {
                        dataGridView2[1, 2].Value = row.Cells[0].Value;

                    }
                    break;
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView4_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index < 3) dataGridView2[1, 7].Value = dataGridView4.CurrentCell.Value;

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index < 3)
            {
                DateTime writeTime = monthCalendar1.SelectionStart;
                DateTime startTime = dateTimePicker1.Value;
                DateTime finTime = startTime.AddDays(31 * ((int)numericUpDown1.Value));
                dataGridView2[1, 1].Value = startTime.ToString("yyyy-MM-dd");
                dataGridView2[1, 2].Value = finTime.ToString("yyyy-MM-dd");
                if (writeTime > startTime) { writeTime = startTime;
                    dataGridView2[1, 4].Value = writeTime.ToString("yyyy-MM-dd");
                }


            }

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            DateTime writeTime = monthCalendar1.SelectionStart;
            DateTime startTime = dateTimePicker1.Value;
           if (index < 3)
            {
                if (writeTime > startTime) writeTime = startTime;
                dataGridView2[1, 4].Value = writeTime.ToString("yyyy-MM-dd");
            }
            else if (index == 8) dataGridView2[1, 0].Value = writeTime.ToString("yyyy-MM-dd");
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            dataGridView2[1,6].Value= (int)numericUpDown2.Value;
        }
    }
    }
    
