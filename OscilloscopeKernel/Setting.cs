//Deng XinYu Build on Sep.27
//ver1.0 on Sep.27
//ver1.1 on Sep.27
//ver1.2 on Sep.28
//ver1.3 on Oct.4
//ver1.4 on Oct.6
//ver1.4.1 on Oct.19
//ver1.4.3 on Dec.30
//ver1.4.4 on Nov.14
//ver2.0.0 on Jan.14/2020


using OscilloscopeCore.Wave;

namespace OscilloscopeCore
{

    ///<summary>
    ///保存阴极射线管相关参数，保存输出、采样的相关参数，保存波形相关信息
    ///</summary>
    public class Setting
    {

        /* 变量声明 */
        private double _brightness;     //辉度
        /// <summary>
        /// 扫描波
        /// </summary>
        public WireSetting Sweep { set; get; }   //扫描波
        /// <summary>
        /// 输入波
        /// </summary>
        public WireSetting Input { set; get; }   //输入波
        /// <summary>
        /// x方向偏移电压
        /// </summary>
        public double Offset_x { set; get; }    //x方向偏移
        /// <summary>
        /// y方向偏移电压
        /// </summary>
        public double Offset_y { set; get; }    //y方向偏移
        /// <summary>
        /// 单点扩展宽度
        /// </summary>
        public int PointHeight { set; get; }      //单点扩展高度
        /// <summary>
        /// 单点扩展高度
        /// </summary>
        public int PointWidth { set; get; }      //单点扩展宽度
        /// <summary>
        /// 加速电压
        /// </summary>
        public double AccelVotage { set; get; }   //加速电压
        /// <summary>
        /// 偏转区长度
        /// </summary>
        public double WheelLenth { get; }    //偏转区长度
        /// <summary>
        /// 偏转区高度/宽度
        /// </summary>
        public double WheelWidth { get; }    //偏转区宽度、高度
        /// <summary>
        /// 滑行区长度
        /// </summary>
        public double SlideLenth { get; }     //滑行区长度 
        /// <summary>
        /// 采样时间间隔
        /// </summary>
        public double SamplePeriod { get; }     //采样时间间隔，单位：ms
        /// <summary>
        /// 是否是AC模式，false时指目前为DC模式
        /// </summary>
        public bool ACMode { get; set; }


        /* 方法声明 */

        //构造函数_参数全部靠输入
        /// <summary>
        /// 构造函数，为各属性指定初值
        /// </summary>
        /// <param name="wheel_lenth"></param>
        /// <param name="wheel_width"></param>
        /// <param name="slide_lenth"></param>
        /// <param name="offset_x"></param>
        /// <param name="offset_y"></param>
        /// <param name="max_input_votage"></param>
        /// <param name="max_sweep_votage"></param>
        /// <param name="accel_votage"></param>
        /// <param name="sweep_freq"></param>
        /// <param name="input_freq"></param>
        /// <param name="point__width"></param>
        /// <param name="point_hetght"></param>
        /// <param name="sample_period"></param>
        /// <param name="brightness"></param>
        public Setting(double wheel_lenth = 400,
                       double wheel_width = 4,
                       double slide_lenth = 4,
                       double offset_x = 0,
                       double offset_y = 0,
                       double max_input_votage = 0,
                       double max_sweep_votage = 0,
                       double accel_votage = 30,
                       int sweep_freq = 50,
                       int input_freq = 50,
                       int point__width = 0,
                       int point_hetght = 0,
                       double sample_period = 0.01234,
                       double brightness = 1.0)
        {
            //阴极射线管尺寸
            this.WheelLenth = wheel_lenth;
            this.WheelWidth = wheel_width;
            this.SlideLenth = slide_lenth;
            //阴极射线管相关电信号
            this.Sweep = new WireSetting(new NoneWave(), max_sweep_votage, sweep_freq, 0);
            this.Input = new WireSetting(new NoneWave(), max_input_votage, input_freq, 0);
            this.Offset_x = offset_x;
            this.Offset_y = offset_y;
            this.AccelVotage = accel_votage;
            this._brightness = brightness > 1 ? brightness % 1 : brightness;
            //采样参数
            this.PointWidth = point__width;
            this.PointHeight = point_hetght;
            this.SamplePeriod = sample_period;

            this.Fresh();
        }

        //构造函数_电波参数直接输入
        /// <summary>
        /// 构造函数，直接使用WireSetting实例进行电信号相关初始化
        /// </summary>
        /// <param name="sweep"></param>
        /// <param name="input"></param>
        /// <param name="wheel_lenth"></param>
        /// <param name="wheel_width"></param>
        /// <param name="slide_lenth"></param>
        /// <param name="offset_x"></param>
        /// <param name="offset_y"></param>
        /// <param name="accel_votage"></param>
        /// <param name="point__width"></param>
        /// <param name="point_hetght"></param>
        /// <param name="sample_period"></param>
        /// <param name="brightness"></param>
        public Setting(WireSetting sweep,
                       WireSetting input,
                       double wheel_lenth = 400,
                       double wheel_width = 4,
                       double slide_lenth = 4,
                       double offset_x = 0,
                       double offset_y = 0,
                       double accel_votage = 30,
                       int point__width = 0,
                       int point_hetght = 0,
                       double sample_period = 0.01234,
                       double brightness = 1.0)
        {
            //阴极射线管尺寸
            this.WheelLenth = wheel_lenth;
            this.WheelWidth = wheel_width;
            this.SlideLenth = slide_lenth;
            //阴极射线管相关电信号
            this.Sweep = sweep;
            this.Input = input;
            this.AccelVotage = accel_votage;
            this.Offset_x = offset_x;
            this.Offset_y = offset_y;
            this._brightness = brightness > 1 ? brightness % 1 : brightness;
            //采样参数
            this.PointWidth = point__width;
            this.PointHeight = point_hetght;
            this.SamplePeriod = sample_period;

            this.Fresh();
        }

        //更新依赖值
        /// <summary>
        /// 更新依赖值
        /// </summary>
        public void Fresh()
        {
            this.Sweep.Fresh();
            this.Input.Fresh();
        }

        /* 属性 */

        /// <summary>
        /// 点的辉度，范围 [0,1]
        /// 当输入的值大于1时会自动忽略整数部分（等于1时不受影响）
        /// </summary>
        public double Brightness
        {
            set
            {
                this._brightness = value > 1 ? value % 1 : value;
            }
            get
            {
                return this._brightness;
            }
        }

        /// <summary>
        /// 文字描述当前设置信息（尚不全）
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string output = "Setting Infomation:\n";
            output += "AccelVotage is " + this.AccelVotage.ToString() + '\n';
            output += "SamplePeriod is " + this.SamplePeriod.ToString() + '\n';
            output += "Sweep is " + this.Sweep.GetWaveInfo() + '\n';
            output += "Input is " + this.Input.GetWaveInfo() + '\n';
            return output;
        }
    }
}