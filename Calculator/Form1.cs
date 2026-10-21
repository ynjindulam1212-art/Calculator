using System;
using System.Globalization;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        // Төлөв хадгалах хувьсагчид
        private double umnuhToo = 0;        // Эхний операнд
        private string uldsenAction = null; // Хүлээгдэж буй үйлдэл (+, -, *, /)
        private bool shineOruulalt = true;  // Шинэ тоо оруулж эхлэх эсэх
        private double sanahOi = 0;         // Санах ойн утга (M)

        private static readonly CultureInfo CI = CultureInfo.InvariantCulture;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Бүх товчийг Button_Click-тэй холбох
        }

        // Товчнуудыг (Panel дотор байсан ч) рекурсив байдлаар хайж холбох
        private void ButtonuudiigHolboh(Control huuhed)
        {
            foreach (Control c in huuhed.Controls)
            {
                if (c is Button b && !string.IsNullOrEmpty(b.Text))
                    b.Click += Button_Click;
                if (c.HasChildren)
                    ButtonuudiigHolboh(c);
            }
        }

        private double Utga => double.Parse(txtDisplay.Text, CI);

        private void Haruulah(double x)
        {
            txtDisplay.Text = x.ToString("G15", CI);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string t = ((Button)sender).Text;

            switch (t)
            {
                // ---- Тоонууд ----
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (shineOruulalt || txtDisplay.Text == "0")
                        txtDisplay.Text = t;
                    else
                        txtDisplay.Text += t;
                    shineOruulalt = false;
                    break;

                case ".":
                    if (shineOruulalt)
                    {
                        txtDisplay.Text = "0.";
                        shineOruulalt = false;
                    }
                    else if (!txtDisplay.Text.Contains("."))
                        txtDisplay.Text += ".";
                    break;

                case "+/-":
                    if (txtDisplay.Text != "0")
                        txtDisplay.Text = txtDisplay.Text.StartsWith("-")
                            ? txtDisplay.Text.Substring(1)
                            : "-" + txtDisplay.Text;
                    break;

                // ---- Үйлдлүүд ----
                case "+":
                case "-":
                case "*":
                case "/":
                    if (uldsenAction != null && !shineOruulalt)
                        Tootsoh();
                    umnuhToo = Utga;
                    uldsenAction = t;
                    shineOruulalt = true;
                    break;

                case "=":
                    if (uldsenAction != null)
                    {
                        Tootsoh();
                        uldsenAction = null;
                        shineOruulalt = true;
                    }
                    break;

                case "sqrt":
                    if (Utga < 0) { Aldaa("Буруу оролт"); break; }
                    Haruulah(Math.Sqrt(Utga));
                    shineOruulalt = true;
                    break;

                case "1/x":
                    if (Utga == 0) { Aldaa("0-д хуваах боломжгүй"); break; }
                    Haruulah(1 / Utga);
                    shineOruulalt = true;
                    break;

                case "%":
                    Haruulah(uldsenAction != null ? umnuhToo * Utga / 100 : 0);
                    shineOruulalt = true;
                    break;

                // ---- Арилгах ----
                case "Backspace":
                    if (shineOruulalt) break;
                    string s = txtDisplay.Text;
                    s = s.Length > 1 ? s.Substring(0, s.Length - 1) : "0";
                    if (s == "-") s = "0";
                    txtDisplay.Text = s;
                    break;

                case "CE":
                    txtDisplay.Text = "0";
                    shineOruulalt = true;
                    break;

                case "C":
                    txtDisplay.Text = "0";
                    umnuhToo = 0;
                    uldsenAction = null;
                    shineOruulalt = true;
                    break;

                // ---- Санах ой ----
                case "MC": sanahOi = 0; break;
                case "MR":
                    Haruulah(sanahOi);
                    shineOruulalt = true;
                    break;
                case "MS": sanahOi = Utga; shineOruulalt = true; break;
                case "M+": sanahOi += Utga; shineOruulalt = true; break;
            }
        }

        // Хүлээгдэж буй үйлдлийг гүйцэтгэх
        private void Tootsoh()
        {
            double hoyor = Utga;
            double ur = 0;

            switch (uldsenAction)
            {
                case "+": ur = umnuhToo + hoyor; break;
                case "-": ur = umnuhToo - hoyor; break;
                case "*": ur = umnuhToo * hoyor; break;
                case "/":
                    if (hoyor == 0) { Aldaa("0-д хуваах боломжгүй"); return; }
                    ur = umnuhToo / hoyor;
                    break;
            }
            Haruulah(ur);
            umnuhToo = ur;
        }

        private void Aldaa(string msg)
        {
            txtDisplay.Text = "0";
            umnuhToo = 0;
            uldsenAction = null;
            shineOruulalt = true;
            MessageBox.Show(msg, "Алдаа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}