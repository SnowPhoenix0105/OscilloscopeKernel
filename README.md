# OscilloscopeKernel

A class library for simulating an oscilloscope. 

## STILL DEVELOPING

Most of this library has been finished, but it is still testing.

Documents will be provided later.

More details will be provided later.


## Quick Start

This project is divided in 2 part: `OscilloscopeKernel`, `OScilloscopeFramework`. `OscilloscopeKernel` is based on .NET Standard, but `OScilloscopeFramework` is based on .NET Framework. 

An oscilloscope is splited to 5 part: `ControlPanel`, `GraphProducer`, `BackgroundDrawer`, `PointDrawer`, `Canvas`. They are defined by interfaces: `IControlPanel`, `IGraphProducer`, `IBackgroundDrawer`, `IPointDrawer`, `ICanvas`. 

Because `ICanvas` is always associated with your own code, so we only provide one implement of it —— `BitmapCanvas`, and it is in the package OScilloscopeFramework.

### .NET Framework

OsciloscopeFramework is based on .NET Framework, 

If you are developing with .NET Framework, you can use BitmapCanvas derectly. You can make it start work like this:

```C#
CathodeRayTubPanel panel = new CathodeRayTubPanel();
panel.XWave.Wave = new SawToothWave(Waves.UNIT_NUMBER_PRO_SECOND / 100, 0.4);
panel.YWave.Wave = new SinWave(Waves.UNIT_NUMBER_PRO_SECOND / 220, 0.4);
panel.PointLength = 1;
panel.PointWidth = 1;
IBackgroundDrawer background_drawer = new CrossBackgroundDrawer(360, 360, background_color, ruler_color, 1);
ConstructorTuple<ICanvas<Bitmap>> canvas_constructor 
    = new ConstructorTuple<ICanvas<Bitmap>>(typeof(BitmapCanvas), 360, 360, background_drawer);
ConstructorTuple<IPointDrawer> single_point_drawer_constructor
                    = new ConstructorTuple<IPointDrawer>(typeof(OvalPointDrawer), 360, 360);
IGraphProducer serial_graph_producer = new SerialProducer(4321, graph_color);
DrivedOscilloscope<Bitmap> drived_serial_oscilloscope
    = new DrivedOscilloscope<Bitmap>(
        canvas_constructor: canvas_constructor,
        point_drawer_constructor: single_point_drawer_constructor,
        graph_producer: serial_graph_producer,
        control_panel: panel);
// we suggest DequeueAndDisplay is a function to dequeue a bitmap from the buffer and display it on the screen
timer = new Timer(o => DequeueAndDisplay(drived_serial_oscilloscope.Buffer), null, 20, 10);
drived_serial_oscilloscope.Start(10_000);
```

Then, you could change the graph with changing the Attributes of `panel`.

### Unity

In unity, maybe you want to produce `Texture2D` object with it, so you need to create a `class` witch implement interface `ICanvas<Texture2D>`.

we suggest that you has already create such a `class` named `TextureCanvas`.

```C#
class OscilloscopeGraphProducer : MonoBehaviour
{
    private UndrivedOscilloscope<Texture2D> oscilloscope;
    private CathodeRayTubPanel panel;

    void Awake()
    {
        panel = new CathodeRayTubPanel();
        panel.XWave.Wave = new SawToothWave(Waves.UNIT_NUMBER_PRO_SECOND / 100, 0.4);
        panel.YWave.Wave = new SinWave(Waves.UNIT_NUMBER_PRO_SECOND / 220, 0.4);
        panel.PointLength = 1;
        panel.PointWidth = 1;
        IBackgroundDrawer background_drawer = new CrossBackgroundDrawer(360, 360, background_color, ruler_color, 1);
        ConstructorTuple<ICanvas<Texture2D>> canvas_constructor 
            = new ConstructorTuple<ICanvas<Texture2D>>(typeof(TextureCanvas), 360, 360, background_drawer);
        ConstructorTuple<IPointDrawer> single_point_drawer_constructor
                            = new ConstructorTuple<IPointDrawer>(typeof(OvalPointDrawer), 360, 360);
        IGraphProducer serial_graph_producer = new SerialProducer(4321, graph_color);
        oscilloscope = new UndrivedOscilloscope<Texture2D>(
            canvas_constructor: canvas_constructor,
            point_drawer_constructor: single_point_drawer_constructor,
            graph_producer: serial_graph_producer,
            control_panel: panel);
        Screen.Buffer = oscilloscope.Buffer;
    }

    void FixedUpdate()
    {
        oscilloscope.Draw(Time.fixedDeltaTime);
    }
}


// in other scripe
class Screen : MonoBehaviour
{
    public static ConcurrentQueue<Texture2D> Buffer { get; set; }

    void FixedUpdate()
    {
        Texture2D new_graph;
        if (Screen.Buffer.TryDequeue(out new_graph))
        {
            // display the new graph, for example:
            RawImage image = gameObject.GetComponent<RawImage>();
            image.texture = new_graph;
        }
    }
}

```
