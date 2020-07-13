# Document of ClassLib OscilloscopeKernel

## Index
  - [Foreword](#Foreword)
  - [OscilloscopeKernel](#OscilloscopeKernel)
  - [SingleThreadOscilloscope](#SingleThreadOscilloscope)
  - [SimpleOscilloscope](#SimpleOscilloscope)
  - [TimeCountedOscilloscope](#TimeCountedOscilloscope)
  - [MultiThreadOscilloscope](#MultiThreadOscilloscope)
  - [UndrivedOscilloscope](#UndrivedOscilloscope)
  - [DrivedOscilloscope](#DrivedOscilloscope)
  - [Wave](#Wave)
  - [](#)


## Foreword
<span id="Foreword"></span>

* if the method or attribute of a certain class that behave the same as the super-class or behave just as the implemented interface requires, it will still be listed again in the document of this certain class, but no details except a `see also`.
* `private` attribute, field, or method will not be listed. `protected` attribute and method will be special marked at the class's attribute-list or method-list. So, the attributes and methods that are listed without special mark are all `public`.
* `protected` and `public` has no difference when it comes to the constructor of a abstract class, so `protected` will not be special marked on this occasion.
* the time unit is defined with [Waves](#Wave\Waves).[UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND). the defaute time unit is $\mu s$ but most of Systerm functions use $ms$ as the time unit, be careful!.




<div style="page-break-after: always;"></div>

## OscilloscopeKernel
<span id="OscilloscopeKernel"></span>
```C#
namespace OscilloscopeKernel
```

Summary:
* main part of oscilloscope-simulation.
* developed in .NET Standard.

|type|name|description|
|:-|:-|:-|
|abstract class|[SingleThreadOscilloscope](#MultiThreadOscilloscope)|an abstract class thar describe an oscilloscope that cannot start a new draw-task while the old one has not finish|
|class|[SimpleOscilloscope](#SimpleOscilloscope)|a SingleThreadOscilloscope with public [Draw](#SimpleOscilloscope\Draw)().|
|class|[TimeCountedOscilloscope](#TimeCountedOscilloscope)|a SingleThreadOscilloscope with public [Draw](#TimeCountedOscilloscope\Draw)() and a built-in watch, which means it doesn't need to delta_time as input.|
|abstract class|[MultiThreadOscilloscope](#MultiThreadOscilloscope)|an abstract class thar describe an oscilloscope that can start a new draw-task while the old one has not finish|
|class|[UndrivedOscilloscope](#UndrivedOscilloscope)|a MultiThreadOscilloscope with public [Draw](#Undrivedoscilloscope\Draw)().|
|class|[DrivedOscilloscope](#DrivedOscilloscope)|a MultiThreadOscilloscope that can produce graphs periodically.|
|namespace|[Wave](#Wave)||
|namespace|[](#)||
|namespace|[](#)||
|namespace|[](#)||
|namespace|[](#)||
|namespace|[](#)||



<div style="page-break-after: always;"></div>

## SingleThreadOscilloscope
<span id="SingleThreadOscilloscope"></span>

```C#
public abstract class SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: none
* interfaces: none
* summary:
  * an oscilloscope that cannot start a new draw-task while the old one has not finished.
  * T is the output type of this oscilloscope.
* remarks
  * this is a abstract class, if you want to use it, please try [SimpleOscilloscope](#SimpleOscilloscope) or [TimeCountedOscilloscope](#TimeCountedOscilloscope).
  * calling [Draw()](#SingleThreadOscilloscope\Draw) to produce and get a new graph.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas<T> canvas, IPointDrawer point_drawer, IGraphProducer graph_producer,IControlPanel control_panel)||
* methods:
  |name|describtion|
  |:-|:-|
  |protected T [Draw](#SingleThreadOscilloscope\Draw)(double delta_time)|produce and get a new graph.|

### constructors:


<span id="SingleThreadOscilloscope\Constructor1"></span>

```C#
protected SingleThreadOscilloscope(
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IGraphProducer graph_producer,
            IControlPanel control_panel)
```

* Summary:
  * create a new oscilloscope.
* Remarks:
  * every input objects should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ICanvas](#Drawing\ICanvas)\<T\> canvas: the canvas that produce the graph.
  * [IPointDrawer](#Drawing\IPointDrawer) point_drawer: the point-drawer the producer will produce the graph with.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas.GraphSize == point_drawer.GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer.IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas.GraphSize != point_drawer.GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer.IsConcurrent
-----------------------

### methods:


<span id="SingleThreadOscilloscope\Draw"></span>

```C#
protected T Draw(double delta_time);
```

* Summary:
  * get the current state of the panel and produce a new graph accoding to this.then return the graph while finish.
* Params:
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method.
* Normal-Behaviour:
  * Post-Condition:
    * a new graph with type T will be produced and return.
---------------------------------------------------------




<div style="page-break-after: always;"></div>

## SimpleOscilloscope
<span id="SimpleOscilloscope"></span>

```C#
public class SimpleOscilloscope<T> : SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: [SingleThreadOscilloscope](#SingleThreadOscilloscope)
* interfaces: none
* summary:
  * the only difference with [SingleThreadOscilloscope](#SingleThreadOscilloscope) is the method [Draw](#SimpleOscilloscope\Draw)() is puiblic.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas<T> canvas, IPointDrawer point_drawer, IGraphProducer graph_producer,IControlPanel control_panel)||
* methods:
  |name|describtion|
  |:-|:-|
  |protected T [Draw](#SingleThreadOscilloscope\Draw)(double delta_time)|produce and get a new graph.|

### constructors:


<span id="SimpleOscilloscope\Constructor1"></span>

```C#
protected SimpleOscilloscope(
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IGraphProducer graph_producer,
            IControlPanel control_panel)
```

* Summary:
  * create a new oscilloscope.
* Remarks:
  * every input objects should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ICanvas](#Drawing\ICanvas)\<T\> canvas: the canvas that produce the graph.
  * [IPointDrawer](#Drawing\IPointDrawer) point_drawer: the point-drawer the producer will produce the graph with.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas.GraphSize == point_drawer.GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer.IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas.GraphSize != point_drawer.GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer.IsConcurrent
-----------------------

### methods:


<span id="SimpleOscilloscope\Draw"></span>

```C#
public T Draw(double delta_time);
```

* Summary:
  * it will call and return the result of [SingleThreadOscilloscope](#SingleThreadOscilloscope).[Draw](#Singlethreadoscilloscope\Draw) directly.
  * get the current state of the panel and produce a new graph accoding to this.then return the graph while finish.
* Params:
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method.
* Normal-Behaviour:
  * Post-Condition:
    * a new graph with type T will be produced and return.
---------------------------------------------------------





<div style="page-break-after: always;"></div>

## TimeCountedOscilloscope
<span id="TimeCountedOscilloscope"></span>

```C#
public class TimeCountedOscilloscope<T> : SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: [SingleThreadOscilloscope](#SingleThreadOscilloscope)
* interfaces: none
* summary:
  * the only difference with [SimpleOscilloscope](#SimpleOscilloscope) is the method [Draw](#TimeCountedOscilloscope\Draw)() will use a built-in watch to get delta-time.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas<T> canvas, IPointDrawer point_drawer, IGraphProducer graph_producer,IControlPanel control_panel)||
* methods:
  |name|describtion|
  |:-|:-|
  |protected T [Draw](#SingleThreadOscilloscope\Draw)()|produce and get a new graph.|

### constructors:


<span id="TimeCountedOscilloscope\Constructor1"></span>

```C#
protected TimeCountedOscilloscope(
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IGraphProducer graph_producer,
            IControlPanel control_panel)
```

* Summary:
  * create a new oscilloscope.
* Remarks:
  * every input objects should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ICanvas](#Drawing\ICanvas)\<T\> canvas: the canvas that produce the graph.
  * [IPointDrawer](#Drawing\IPointDrawer) point_drawer: the point-drawer the producer will produce the graph with.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas.GraphSize == point_drawer.GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer.IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas.GraphSize != point_drawer.GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer.IsConcurrent
-----------------------

### methods:


<span id="TimeCountedOscilloscope\Draw"></span>

```C#
public T Draw();
```

* Summary:
  * it will get delta_time with built-in watch.
  * it will call and return the result of [SingleThreadOscilloscope](#SingleThreadOscilloscope).[Draw](#Singlethreadoscilloscope\Draw) directly.
  * get the current state of the panel and produce a new graph accoding to this.then return the graph while finish.
* Params:
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method.
* Normal-Behaviour:
  * Post-Condition:
    * a new graph with type T will be produced and return.
---------------------------------------------------------




<div style="page-break-after: always;"></div>

## MultiThreadOscilloscope
<span id="MultiThreadOscilloscope"></span>

```C#
public abstract class MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: none
* interfaces: none
* summary:
  * an oscilloscope that can start a new draw-task while the old one has not finished.
  * T is the output type of this oscilloscope.
* remarks
  * this is a abstract class, if you want to use it, please try [UndrivedOscilloscope](#UndrivedOscilloscope) or [DrivedOscilloscope](#DrivedOscilloscope).
  * calling [Draw()](#MultiThreadOscilloscope\Draw) to start a draw-task, and after the draw-task is complete, a new graph will be put into [Buffer](#MultiThreadOscilloscope\Buffer).
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* constructors:
  |name|describtion|
  |:-|:-|
  |[MultiThreadOscilloscope](#MultiThreadOscilloscope\Constructor1)(ConstructorTuple\<ICanvas\<T\>\> canvas_constructor,ConstructorTuple\<IPointDrawer\> point_drawer_constructor,IRulerDrawer ruler_drawer,IGraphProducer graph_producer,IControlPanel control_panel,ConcurrentQueue\<T\> buffer = null)||
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |ConcurrentQueue\<T\>|[Buffer](#MultiThreadOscilloscope\Buffer)|G|the productions of this oscilloscope will be put into this buffer.|
* methods:
  |name|describtion|
  |:-|:-|
  |protected void [Draw](#MultiThreadOscilloscope\Draw)(double)|get the current state of the panel and produce a new graph accoding to this.then put the new graph into [Buffer](MultiThreadOscilloscope\Buffer)|

### constructors:

<span id="MultiThreadOscilloscope\Constructor1"></span>

```C#
public MultiThreadOscilloscope(
    ConstructorTuple<ICanvas<T>> canvas_constructor,
    ConstructorTuple<IPointDrawer> point_drawer_constructor,
    IGraphProducer graph_producer,
    IControlPanel control_panel,
    ConcurrentQueue<T> buffer = null)
```

* Summary:
  * create a new Oscilloscope.
* Remarks:
  * the control_panel and graph_producer should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[ICanvas](#Drawing\ICanvas)\<T\>\> canvas_constructor: a ConstructorTuple that can create new ICanvas.
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[IPointDrawer](#Drawing\IPointDrawer)\> point_drawer_constructor: a ConstructorTuple that can create new IPointDrawer.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas_constructor.NewInstance().GraphSize == point_drawer_constructor.NewInstance().GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer_constructor.NewInstance().IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas_constructor.NewInstance().GraphSize != point_drawer_constructor.NewInstance().GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer_constructor.NewInstance().IsConcurrent
---------------------------------------------------------

### attributes:

<span id="MultiThreadOscilloscope\Buffer"></span>

```C#
public ConcurrentQueue<T> Buffer { get; }
```

* Summary:
  * the productions of this oscilloscope will be put into this buffer.
  * the reference of buffer will never change.

---------------------------------------------------------



### methods:

<span id="MultiThreadOscilloscope\Draw"></span>

```C#
protected void Draw(double delta_time);
```

* Summary:
  * get the current state of the panel and produce a new graph accoding to this.then put the new graph into [Buffer](MultiThreadOscilloscope\Buffer)
* Params:
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method.
* Normal-Behaviour:
  * Post-Condition:
    * a new graph with type T will be produced and put into [Buffer](#MultiThreadOscilloscope\Buffer)




<div style="page-break-after: always;"></div>

## UndrivedOscilloscope
<span id="UndrivedOscilloscope"></span>

```C#
public class UndrivedOscilloscope<T> : MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: MultiThreadOscilloscope\<T\>
* interfaces: none
* summary:
  * the only difference with [MultiThreadOscilloscope](#MultiThreadOscilloscope) is that the [Draw](#MultiThreadOscilloscope\Draw)() of [UndrivedOscilloscope](#UndrivedOscilloscope) is public.
* constructors:
  |name|describtion|
  |:-|:-|
  |[UndrivedOscilloscope](#)(ConstructorTuple\<ICanvas\<T\>\> canvas_constructor,ConstructorTuple\<IPointDrawer\> point_drawer_constructor,IGraphProducer graph_producer,IControlPanel control_panel,ConcurrentQueue\<T\> buffer = null)||
* methods:
  |name|describtion|
  |:-|:-|
  |void [Draw](#UndrivedOscilloscope\Draw)(double)|call [MultiThreadOscilloscope](#Multithreadoscilloscope).[Draw](#MultiThreadOscilloscope\Draw)() directly.|

### constructors:


<span id="UndrivedOscilloscope\Constructor1"></span>

```C#
public UndrivedOscilloscope(
    ConstructorTuple<ICanvas<T>> canvas_constructor,
    ConstructorTuple<IPointDrawer> point_drawer_constructor,
    IGraphProducer graph_producer,
    IControlPanel control_panel,
    ConcurrentQueue<T> buffer = null)
```

* Summary:
  * create a new Oscilloscope.
  * the same as [MultiThreadOscilloscope](#MultiThreadOscilloscope\Constructor1).
* Remarks:
  * the control_panel and graph_producer should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[ICanvas](#Drawing\ICanvas)\<T\>\> canvas_constructor: a ConstructorTuple that can create new ICanvas.
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[IPointDrawer](#Drawing\IPointDrawer)\> point_drawer_constructor: a ConstructorTuple that can create new IPointDrawer.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas_constructor.NewInstance().GraphSize == point_drawer_constructor.NewInstance().GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer_constructor.NewInstance().IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas_constructor.NewInstance().GraphSize != point_drawer_constructor.NewInstance().GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer_constructor.NewInstance().IsConcurrent
---------------------------------------------------------


### methods:


<span id="UndrivedOscilloscope\Draw"></span>

```C#
public void Draw(double delta_time);
```

* Summary:
  * it will call [MultiThreadOscilloscope](#Multithreadoscilloscope).[Draw](#MultiThreadOscilloscope\Draw)() directly.
  * get the current state of the panel and produce a new graph accoding to this.then put the new graph into [Buffer](MultiThreadOscilloscope\Buffer)
* Params:
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method.
* Normal-Behaviour:
  * Post-Condition:
    * a new graph with type T will be produced and put into [Buffer](#MultiThreadOscilloscope\Buffer)





<div style="page-break-after: always;"></div>

## DrivedOscilloscope
<span id="DrivedOscilloscope"></span>

```C#
public class DrivedOscilloscope<T> : MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* supers: MultiThreadOscilloscope\<T\>
* interfaces: none
* summary:
  * a multi-thread oscilloscope that contains a built-in timer.
  * it will produce graphs periodically and put them into the [Buffer](#Multithreadoscilloscope\Buffer).
* constructors:
  |name|describtion|
  |:-|:-|
  |[DrivedOscilloscope](#DrivedOscilloscope\Constructor1)(ConstructorTuple\<ICanvas\<T\>\> canvas_constructor,ConstructorTuple\<IPointDrawer\> point_drawer_constructor,IGraphProducer graph_producer,IControlPanel control_panel,ConcurrentQueue\<T\> buffer = null)||
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |bool|[IsRunning](#DrivedOscilloscope\IsRunning)|G|marks wheather this oscilloscope is running|
* methods:
  |name|describtion|
  |:-|:-|
  |void [Start](#DrivedOscilloscope\Start)(int delta_time)|start to produce graphs periodically.|
  |void [End](#DrivedOscilloscope\End)()|stop this oscilloscope.|

### constructors:

<span id="DrivedOscilloscope\Constructor1"></span>

```C#
public DrivedOscilloscope(
    ConstructorTuple<ICanvas<T>> canvas_constructor,
    ConstructorTuple<IPointDrawer> point_drawer_constructor,
    IGraphProducer graph_producer,
    IControlPanel control_panel,
    ConcurrentQueue<T> buffer = null)
```

* Summary:
  * create a new Oscilloscope.
  * the same as [MultiThreadOscilloscope](#MultiThreadOscilloscope\Constructor1).
* Remarks:
  * the control_panel and graph_producer should not be used by other oscilloscope at the same time.
  * no attribute or method will be provided to get the panel that this oscilloscope is using, so you need to handle the reference of it by yourself.
* Params:
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[ICanvas](#Drawing\ICanvas)\<T\>\> canvas_constructor: a ConstructorTuple that can create new ICanvas.
  * [ConstructorTuple](#Tools\ConstructorTuple)\<[IPointDrawer](#Drawing\IPointDrawer)\> point_drawer_constructor: a ConstructorTuple that can create new IPointDrawer.
  * [IGraphProducer](#Producer\IGraphProducer) graph_producer: a certain GraphProducer, MultiThreadOscilloscope requirs a concurrent producer, which means producer.[Produce](#Drawing\IGraphProducer\Produce)() can be called by different thread.
  * [IControlPanel](#Producer\IControlPanel) control_panel: the user-interface of this oscilloscope.
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope, if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * canvas_constructor.NewInstance().GraphSize == point_drawer_constructor.NewInstance().GraphSize
    * !graph_producer.RequireConcurrentDrawer || point_drawer_constructor.NewInstance().IsConcurrent
* Exception-Behaviour:
  * Exception: OscillocopeBuildException with inner-exception: DifferentGraphSizeException
    * canvas_constructor.NewInstance().GraphSize != point_drawer_constructor.NewInstance().GraphSize
  * Exception: OscillocopeBuildException
    * graph_producer.RequireConcurrentDrawer && !point_drawer_constructor.NewInstance().IsConcurrent
---------------------------------------------------------

### attributes:

<span id="DrivedOscilloscope\IsRunning"></span>

```C#
public bool IsRunning { get; }
```

* Summary:
  * marks wheather this oscilloscope is running
* Remarks
  * while IsRunning is true, ths oscilloscope will produce a new graph and put it into the [Buffer](#Multithreadoscilloscope\Buffer) periodically.
* Getter


### methods:

<span id="DrivedOscilloscope\Start"></span>

```C#
public void Start(int delta_time);
```

* Summary:
  * the oscilloscope start to run, which means it will put a new graph into the [Buffer](#Multithreadoscilloscope\Buffer) every `delta_time`.
* Remarks:
  * be careful about the time unit of delta_time. the time unit is still difined with [Waves](#Wave\Waves).[UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND).
* Params:
  * int delta_time: the period that this oscilloscope produce a new graph and put into the [Buffer](#Multithreadoscilloscope\Buffer).
* Normal-Behaviour:
  * Pre-Condition:
    * IsRunning == true
  * Post-Condition:
    * stop and then restart to run.
    * IsRunning == true
* Normal-Behaviour:
  * Pre-Condition:
    * IsRunning == false
  * Post-Condition:
    * start to run.
    * IsRunning == true
---------------------------------------------------------



<span id="DrivedOscilloscope\End"></span>

```C#
public void End()
```

* Summary:
  * stop this oscilloscope.
* Remarks:
  * if the oscilloscope is not running, nothing will happen.
* Normal-Behaviour:
  * Pre-Condition:
    * IsRunning == true
  * Post-Condition:
    * the oscilloscope will stop producing graphs periodically
    * IsRunning == false
* Normal-Behaviour:
  * Pre-Condition:
    * IsRunning == false
  * Post-Condition:
    * nothing will happen
---------------------------------------------------------
















<div style="page-break-after: always;"></div>

## Wave
<span id="Wave"></span>
```C#
namespace OscilloscopeKernel.Wave
```

Summary:
* tools to describe electric waves with time and voltage.

|type|name|description|
|:-|:-|:-|
|interface|[IWave](#IWave)||
||[](#)||
||[](#)||
||[](#)||





<div style="page-break-after: always;"></div>

## 
<span id="IWave"></span>

```C#
    public interface IWave
    {
        double MeanVoltage { get; }

        int Period { get; }

        double Voltage(double phase);
    }
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* supers: none
* interfaces: none
* summary:
  * describe a periodic wave with time, phase and voltage.
* remarks
  * every object that implement this interface should be immutable- object. if you want a wave that can be changed, you'd better not let it implement IWave. you could add a `GetStateShot()` method to return an IWave at certain time, just like how [WaveFixer](#Wave\WaveFixer) do.
  * this wave can be described with a function $f(t)$. the voltage at time $t$ is $f(t)$, and $\exist T, s.t.$ $f(t) = f(t+T)$
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#IWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#IWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#IWave\Voltage)(double)|return the voltage of this wave with certain phase|


### attributes:


<span id="IWave\MeanVoltage"></span>

```C#
double MeanVoltage { get; }
```

* Summary:
  * the mean voltage of this wave.
* Remarks
  * definition: ${\rm MeanVoltage}=\int_0^1 {\rm Voltage}(p){\rm d}p$
  * [Waves](#Wave\Waves).[CalculateMeanVoltage](#Wave\Waves\CalculateMeanVoltage)() can calculate the meanvoltage with difinition.
* Invarient:
  * ${\rm MeanVoltage}=\int_0^1 {\rm Voltage}(p){\rm d}p$
* Getter
---------------------------------------------------------


<span id="Wave\IWave\Period"></span>

```C#
int Period { get; }
```

* Summary:
  * the period of this wave.
* Remarks
  * definition: $\forall time, phase={t \over {\rm Period}}$ ${\rm mod}$ $1$
  * the voltage at time $t$ is the same as the voltage at time $t + {\rm Period}$
* Getter
---------------------------------------------------------

### methods:




<span id="Wave\IWave\Voltage"></span>

```C#
double Voltage(double phase);
```

* Summary:
  * 
* Remarks:
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














<div style="page-break-after: always;"></div>

## Tools
<span id="Tools"></span>
```C#
namespace OscilloscopeKernel.Tools
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||

















<div style="page-break-after: always;"></div>

## Drawing
<span id="Drawing"></span>
```C#
namespace OscilloscopeKernel.Drawing
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||























<div style="page-break-after: always;"></div>

## Producer
<span id="Producer"></span>
```C#
namespace OscilloscopeKernel.Producer
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||



















<div style="page-break-after: always;"></div>

## Exceptions
<span id="Exceptions"></span>
```C#
namespace OscilloscopeKernel.Exceptions
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||



















<div style="page-break-after: always;"></div>

## OscilloscopeFramework
<span id="OscilloscopeFramework"></span>
```C#
namespace OscilloscopeFramework
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||