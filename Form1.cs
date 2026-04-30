using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SimplePaint
{
    //git리포지토리를 늦게 만들어서 다시 커밋
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //기본색 검정색
        Color selectedColor = Color.Black;

        // 현재 선택된 두께를 저장 (기본값 1)
        int lineThickness = 1;

        // 현재 선택된 도형 (기본값 직선)
        string shape = "Line";

        private void btnLine_Click(object sender, EventArgs e)
        {
            shape = "Line";
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            shape = "Rectangle";
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            shape = "Circle";
        }

        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string colorName = cmbColor.SelectedItem.ToString();

            switch (colorName)
            {
                case "Black": selectedColor = Color.Black; break;
                case "Red": selectedColor = Color.Red; break;
                case "Blue": selectedColor = Color.Blue; break;
                case "Green": selectedColor = Color.Green; break;
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            cmbColor.Items.Clear();
            cmbColor.Items.AddRange(new string[] { "Black", "Red", "Blue", "Green" });

            cmbColor.SelectedIndex = 0; // 기본값으로 'Black' 선택
        }

        private void trbLineWidth_Scroll(object sender, EventArgs e)
        {
            lineThickness = trbLineWidth.Value;
        }
    }
}
