using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace SimplePaint
{
    public partial class Form1 : Form
    {
        Point startPos;           // 마우스 클릭 시작 지점 저장
        bool isDrawing = false;    // 현재 그리기 상태인지 체크
        Point currentPos;           // 마우스 이동 중 현재 위치 저장
        // 기존에 만든 변수들도 여기 같이 있어야 합니다.
        Color selectedColor = Color.Black;
        int lineThickness = 1;
        string shape = "Line";

        public Form1()
        {
            InitializeComponent();
        }


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

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPos = e.Location; // 마우스가 눌린 현재 위치 저장
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing) return;

            using (Graphics g = picCanvas.CreateGraphics())
            {
                // 선을 부드럽게 만드는 옵션
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 현재 설정된 색상과 두께로 펜 생성
                Pen pen = new Pen(selectedColor, lineThickness);

                // 사각형/원 계산을 위한 좌표 처리
                int width = Math.Abs(e.X - startPos.X);
                int height = Math.Abs(e.Y - startPos.Y);
                int left = Math.Min(startPos.X, e.X);
                int top = Math.Min(startPos.Y, e.Y);

                // 도형 종류에 따라 그리기
                if (shape == "Line")
                {
                    g.DrawLine(pen, startPos, e.Location);
                }
                else if (shape == "Rectangle")
                {
                    g.DrawRectangle(pen, left, top, width, height);
                }
                else if (shape == "Circle")
                {
                    g.DrawEllipse(pen, left, top, width, height);
                }
            }

            isDrawing = false; // 그리기 종료
    }
    }
}
