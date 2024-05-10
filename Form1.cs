using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsMusic1
{
    public partial class Form1 : Form
    {
        private readonly string[] supportedExtensions = { ".mp3", ".wav", ".flac", ".ogg" };

        public Form1()
        {
            InitializeComponent();
        }

        // 浏览文件
        private void BrowseFiles()
        {
            // 设置文件对话框的过滤器
            openFileDialog1.Filter = "选择音频|*.mp3;*.wav;*.flac;*.ogg";
            // 允许多选
            openFileDialog1.Multiselect = true;

            // 如果用户选择了文件
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 清空列表框
                listBox1.Items.Clear();
                // 将选择的文件添加到列表框中
                foreach (var file in openFileDialog1.FileNames)
                {
                    listBox1.Items.Add(file);
                }
            }
        }

        // 播放音乐
        private void PlayMusic(string filename)
        {
            // 获取文件扩展名
            string extension = Path.GetExtension(filename);
            // 如果是Ogg文件
            if (extension == ".ogg")
            {
                // 播放Ogg文件
                PlayOggFile(filename);
            }
            else
            {
                // 使用Windows Media Player播放其他格式的音乐文件
                axWindowsMediaPlayer1.URL = filename;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                // 显示正在播放的歌曲名
                label1.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        // 播放Ogg文件
        private void PlayOggFile(string oggFilePath)
        {
            // 使用NAudio解码和播放Ogg文件
            using (var vorbisReader = new VorbisWaveReader(oggFilePath))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(vorbisReader);
                    outputDevice.Play();
                    // 等待播放完毕
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }

        // 添加歌曲
        private void button1_Click(object sender, EventArgs e)
        {
            // 打开文件对话框，选择音频文件
            BrowseFiles();
        }

        // 列表框选项改变的事件处理方法
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 如果有选中的项
            if (listBox1.SelectedIndex >= 0)
            {
                // 播放选中的音乐文件
                PlayMusic(listBox1.SelectedItem.ToString());
            }
        }

        // 音量滑动条滚动
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // 调整音量大小
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
            // 更新音量显示标签
            label2.Text = trackBar1.Value + "%";
        }

        // 停止播放
        private void button2_Click(object sender, EventArgs e)
        {
            // 停止播放音乐
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        // 下一首
        private void button3_Click(object sender, EventArgs e)
        {
            // 如果列表框中有音乐文件
            if (listBox1.Items.Count > 0)
            {
                // 计算下一个音乐文件的索引
                int nextIndex = (listBox1.SelectedIndex + 1) % listBox1.Items.Count;
                // 设置列表框选中下一个音乐文件
                listBox1.SelectedIndex = nextIndex;
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
