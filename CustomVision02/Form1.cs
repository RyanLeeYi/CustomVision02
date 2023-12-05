using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CustomVision02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // 選擇圖檔
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imgPath = openFileDialog1.FileName;
                    pictureBox1.Image = new Bitmap(imgPath);

                    //api的網址
                    string apiUrl = @"";

                    //api的金鑰
                    string apiKey = "";

                    //建立HttpClient物件client並指定服務金鑰
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Prediction-Key", apiKey);

                    // 指定本機欲分析的影像
                    FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
                    ByteArrayContent content = new ByteArrayContent(byteData);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // 採 REST API 呼叫並傳送影像
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // 取得結果，結果以JSON字串傳回
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    // 格式化JSON資料，並將JSON顯示於richTextBox1
                    richTextBox1.Text = JObject.Parse(jsonStr).ToString();

                    // 重新繪製pictureBox1的內容
                    pictureBox1.Refresh();

                    int intVWidth = pictureBox1.Width;
                    int intVHeight = pictureBox1.Height;

                    Graphics g = pictureBox1.CreateGraphics();
                    Pen pen = new Pen(Color.Blue, 3);
                    int left, top, width, height;

                    Info info = JsonConvert.DeserializeObject<Info>(jsonStr);
                    string str = "辨識如下：\n";
                    for (int i = 0; i < info.predictions.Count; i++)
                    {
                        if (double.Parse(info.predictions[i].probability) >= 0.5)
                        {
                            str += $"\t{info.predictions[i].tagName}\t{info.predictions[i].probability}\n";
                            //找出pictureBox1畫矩形的範圍(left, top, width, height)
                            left = (int)(intVWidth * info.predictions[i].boundingBox.left);
                            top = (int)(intVHeight * info.predictions[i].boundingBox.top);
                            width = (int)(intVWidth * info.predictions[i].boundingBox.width);
                            height = (int)(intVHeight * info.predictions[i].boundingBox.height);
                            g.DrawRectangle(pen, left, top, width, height);
                        }
                    }
                    richTextBox2.Text = str;
                }
                catch (Exception ex)
                {
                    richTextBox1.Text = ex.Message;
                }
            }
        }
    }
}