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
            // ��ܹ���
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imgPath = openFileDialog1.FileName;
                    pictureBox1.Image = new Bitmap(imgPath);

                    //api�����}
                    string apiUrl = @"";

                    //api�����_
                    string apiKey = "";

                    //�إ�HttpClient����client�ë��w�A�Ȫ��_
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Prediction-Key", apiKey);

                    // ���w���������R���v��
                    FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    byte[] byteData = binaryReader.ReadBytes((int)fileStream.Length);
                    ByteArrayContent content = new ByteArrayContent(byteData);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // �� REST API �I�s�öǰe�v��
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // ���o���G�A���G�HJSON�r��Ǧ^
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    // �榡��JSON��ơA�ñNJSON��ܩ�richTextBox1
                    richTextBox1.Text = JObject.Parse(jsonStr).ToString();

                    // ���sø�spictureBox1�����e
                    pictureBox1.Refresh();

                    int intVWidth = pictureBox1.Width;
                    int intVHeight = pictureBox1.Height;

                    Graphics g = pictureBox1.CreateGraphics();
                    Pen pen = new Pen(Color.Blue, 3);
                    int left, top, width, height;

                    Info info = JsonConvert.DeserializeObject<Info>(jsonStr);
                    string str = "���Ѧp�U�G\n";
                    for (int i = 0; i < info.predictions.Count; i++)
                    {
                        if (double.Parse(info.predictions[i].probability) >= 0.5)
                        {
                            str += $"\t{info.predictions[i].tagName}\t{info.predictions[i].probability}\n";
                            //��XpictureBox1�e�x�Ϊ��d��(left, top, width, height)
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