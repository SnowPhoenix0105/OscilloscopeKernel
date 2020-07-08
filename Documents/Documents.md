# OscilloscopeCore

## 目录
* [Wave](#Wave)
  * [WaveDriver](#Wave/WaveDriver)


## OscilloscopeCore
<span id="OscilloscopeCore"></span>
```C#
namespace OscilloscopeCore
```
|type|name|description|
|:-|:-|:-|
||[](#)||
|namespace|[Wave](#Wave)||
||[](#)||
||[](#)||




## 
<span id="MultiThreadOscilloscope"></span>

```C#
public abstract class MultiThreadOscilloscope<T>
```

* namespace: [OscilloscopeCore](#OscilloscopeCore).[]()
* supers: none
* interfaces: none
* summary:
  * T is the output type of this oscilloscope
* constructors:
  |name|describtion|
  |:-|:-|
  |[MultiThreadOscilloscope](#MultiThreadOscilloscope\Constructor1)(ConstructorTuple<ICanvas<T>> canvas_constructor,ConstructorTuple<IPointDrawer> point_drawer_constructor,IRulerDrawer ruler_drawer,IGraphProducer graph_producer,IControlPanel control_panel,ConcurrentQueue<T> buffer = null)||
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |ConcurrentQueue\<T\>|[Buffer](#MultiThreadOscilloscope\Buffer)|G|the productions of this oscilloscope will be put into this buffer.|
* methods:
  |name|describtion|
  |:-|:-|
  |[](#)()||
  |[](#)()||
  |[](#)()||
  |[](#)()||
  |[](#)()||
  |[](#)()||

### constructors:

<span id="MultiThreadOscilloscope\Constructor1"></span>

```C#
public MultiThreadOscilloscope(
    ConstructorTuple<ICanvas<T>> canvas_constructor,
    ConstructorTuple<IPointDrawer> point_drawer_constructor,
    IRulerDrawer ruler_drawer,
    IGraphProducer graph_producer,
    IControlPanel control_panel,
    ConcurrentQueue<T> buffer = null)
```

* Summary:
  * 
* Params:
  * 
* Return:
  * 
* Normal-Behaviour:
  * Pre-Condition:
    * 
  * Post-Condition:
    * 
  * Side-Effect:
    * 
* Exception-Behaviour:
  * Exception:
    * 
  * Exception:
    * 
---------------------------------------------------------

### attributes:

<span id="MultiThreadOscilloscope\Buffer"></span>

```C#
public ConcurrentQueue<T> Buffer { get; }
```

* Summary:
  * the productions of this oscilloscope will be put into this buffer.
  * it will never change.
---------------------------------------------------------

### methods:

## Wave
<span id="Wave"></span>
```C#
namespace OscilloscopeCore.Wave
```
|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||

