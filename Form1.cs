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
        float zoomScale = 1.0f;

        Bitmap bitmap;

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
            bitmap = new Bitmap(picCanvas.Width, picCanvas.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White); // 배경을 흰색으로 초기화
            }
            picCanvas.Image = bitmap; // 픽처박스에 비트맵 연결
        }

        private void trbLineWidth_Scroll(object sender, EventArgs e)
        {
            lineThickness = trbLineWidth.Value;
        }

        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPos = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
            startPos = e.Location; // 마우스가 눌린 현재 위치 저장
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            Point endPos = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
            if (!isDrawing) return;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Pen pen = new Pen(selectedColor, lineThickness);

                int width = Math.Abs(e.X - startPos.X);
                int height = Math.Abs(e.Y - startPos.Y);
                int left = Math.Min(startPos.X, e.X);
                int top = Math.Min(startPos.Y, e.Y);

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

            isDrawing = false;

            // 비트맵에 그린 내용을 화면에 즉시 반영합니다.
            picCanvas.Invalidate();
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "이미지 저장하기";
                sfd.OverwritePrompt = true;
                sfd.Filter = "PNG 파일 (*.png)|*.png|JPEG 파일 (*.jpg)|*.jpg|Bitmap 파일 (*.bmp)|*.bmp";
                sfd.DefaultExt = "png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // 선택한 확장자에 맞춰 포맷 결정
                    ImageFormat format = ImageFormat.Png;
                    string extension = System.IO.Path.GetExtension(sfd.FileName).ToLower();

                    switch (extension)
                    {
                        case ".jpg": format = ImageFormat.Jpeg; break;
                        case ".bmp": format = ImageFormat.Bmp; break;
                    }

                    // 비트맵 저장
                    bitmap.Save(sfd.FileName, format);
                }
            }
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                currentPos = new Point((int)(e.X / zoomScale), (int)(e.Y / zoomScale));
                currentPos = e.Location; // 마우스 이동 중 좌표 저장
                picCanvas.Invalidate();  // 픽처박스의 Paint 이벤트를 강제로 발생시킴
            }
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            if (isDrawing)
            {
                Graphics g = e.Graphics;
                // 드래그 중임을 보여주기 위해 회색 점선 펜 생성 (선택 사항)
                Pen guidePen = new Pen(selectedColor, lineThickness);
                guidePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                int x = (int)(Math.Min(startPos.X, currentPos.X) * zoomScale);
                int y = (int)(Math.Min(startPos.Y, currentPos.Y) * zoomScale);
                int w = (int)(Math.Abs(startPos.X - currentPos.X) * zoomScale);
                int h = (int)(Math.Abs(startPos.Y - currentPos.Y) * zoomScale);

                if (shape == "Line")
                    g.DrawLine(guidePen, startPos, currentPos);
                else if (shape == "Rectangle")
                    g.DrawRectangle(guidePen, x, y, w, h);
                else if (shape == "Circle")
                    g.DrawEllipse(guidePen, x, y, w, h);
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap loadedImg = new Bitmap(ofd.FileName);
                    bitmap = new Bitmap(loadedImg.Width, loadedImg.Height);
                    using (Graphics g = Graphics.FromImage(bitmap)) { g.DrawImage(loadedImg, 0, 0); }
                    picCanvas.Size = bitmap.Size; // 픽처박스 크기를 이미지에 맞춤
                    picCanvas.Image = bitmap;
                    zoomScale = 1.0f;
                }
            }
        }

        private void picCanvas_Click(object sender, EventArgs e)
        {

        }
        private void UpdateZoom(float factor)
        {
            zoomScale *= factor; // 배율을 곱함 (예: 1.0 * 1.1 = 1.1배)

            // 실제 비트맵 크기에 배율을 곱해서 픽처박스 크기를 진짜로 키움
            picCanvas.Width = (int)(bitmap.Width * zoomScale);
            picCanvas.Height = (int)(bitmap.Height * zoomScale);
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            UpdateZoom(1.1f); // 10% 확대
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            UpdateZoom(0.9f); // 10% 축소
        }
    }
}
