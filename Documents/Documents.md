# Document of ClassLib OscilloscopeKernel

## Index
  * [Foreword](#Foreword)
  * [OscilloscopeKernel](#OscilloscopeKernel)
  * [SingleThreadOscilloscope](#SingleThreadOscilloscope)
  * [SimpleOscilloscope](#SimpleOscilloscope)
  * [TimeCountedOscilloscope](#TimeCountedOscilloscope)
  * [MultiThreadOscilloscope](#MultiThreadOscilloscope)
  * [UndrivedOscilloscope](#UndrivedOscilloscope)
  * [DrivedOscilloscope](#DrivedOscilloscope)
  * [Wave](#Wave)


<span id="Foreword"></span>

## Foreword

* if the method or attribute of a certain class that behave the same as the super-class or behave just as the implemented interface requires, it will still be listed again in the document of this certain class, but no details except a `see also`.
* `private` attribute, field, or method will not be listed. `protected` attribute and method will be special marked at the class's attribute-list or method-list. So, the attributes and methods that are listed without special mark are all `public`.
* `protected` and `public` has no difference when it comes to the constructor of a abstract class, so `protected` will not be special marked on this occasion.
* static method and static attribute will be special marked, except the methods and attributes of a static class.
* the time unit is defined with [Waves](#Wave\Waves).[UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND). the defaute time unit is $\mu s$ but most of Systerm functions use $ms$ as the time unit, be careful!.
* for attributes-list of classes or interfaces, in accessor row:
  |symbol|meaning|
  |:-|:-|
  |G|only has a public Getter|
  |S|only has a public Setter|
  |g|only has a protected Getter|
  |s|only has a protected Setter|
  |GS|has both public Getter and public Setter|
  |gS|has protected Getter and public Setter|
  |Gs|has public Getter and protected Setter|
  |readonly|is a readonly field (if a field is not readonly, this classlib make sure it is private)|
* parameter's type, but name, will not provided in the method-list in a class or interface. if you need the name of parameters click the method name or scroll down to see the details of this method. 




<div style="page-break-after: always;"></div>

<span id="OscilloscopeKernel"></span>

## OscilloscopeKernel

```C#
namespace OscilloscopeKernel
```

Summary:
* main part of oscilloscope-simulation.
* developed in .NET Standard.

|type|name|description|
|:-|:-|:-|
|abstract class|[SingleThreadOscilloscope](#SingleThreadOscilloscope)|an abstract class thar describe an oscilloscope that cannot start a new draw-task while the old one has not finish|
|class|[SimpleOscilloscope](#SimpleOscilloscope)|a SingleThreadOscilloscope with public [Draw](#SimpleOscilloscope\Draw)().|
|class|[TimeCountedOscilloscope](#TimeCountedOscilloscope)|a SingleThreadOscilloscope with public [Draw](#TimeCountedOscilloscope\Draw)() and a built-in watch, which means it doesn't need to delta_time as input.|
|abstract class|[MultiThreadOscilloscope](#MultiThreadOscilloscope)|an abstract class thar describe an oscilloscope that can start a new draw-task while the old one has not finish|
|class|[UndrivedOscilloscope](#UndrivedOscilloscope)|a MultiThreadOscilloscope with public [Draw](#Undrivedoscilloscope\Draw)().|
|class|[DrivedOscilloscope](#DrivedOscilloscope)|a MultiThreadOscilloscope that can produce graphs periodically.|
|namespace|[Wave](#Wave)|tools to describe electric waves with time and voltage.|
|namespace|[Tools](#Tools)||
|namespace|[Drawing](#Drawing)||
|namespace|[Producer](#Producer)||
|namespace|[Exceptions](#Exceptions)||



<div style="page-break-after: always;"></div>

<span id="SingleThreadOscilloscope"></span>

## SingleThreadOscilloscope

```C#
public abstract class SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ SingleThreadOscilloscope\<T\>
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
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas\<T\>, IPointDrawer,IGraphProducer,IControlPanel)||
* methods:
  |name|describtion|
  |:-|:-|
  |protected T [Draw](#SingleThreadOscilloscope\Draw)(double)|produce and get a new graph.|

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
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method. it should not be negative.
* Normal-Behaviour:
  * Pre-condition:
    * delta_time >= 0.
  * Post-Condition:
    * a new graph with type T will be produced and return.
---------------------------------------------------------




<div style="page-break-after: always;"></div>

<span id="SimpleOscilloscope"></span>

## SimpleOscilloscope

```C#
public class SimpleOscilloscope<T> : SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ [SingleThreadOscilloscope](#SingleThreadOscilloscope)\<T\> $\rarr$ SimpleOscilloscope\<T\>
* interfaces: none
* summary:
  * the only difference with [SingleThreadOscilloscope](#SingleThreadOscilloscope) is the method [Draw](#SimpleOscilloscope\Draw)() is puiblic.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas\<T\>, IPointDrawer,IGraphProducer,IControlPanel)||
* methods:
  |name|describtion|
  |:-|:-|
  |protected T [Draw](#SingleThreadOscilloscope\Draw)(double)|produce and get a new graph.|

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
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method. it should not be negative.
* Normal-Behaviour:
  * Pre-condition:
    * delta_time >= 0.
  * Post-Condition:
    * a new graph with type T will be produced and return.
---------------------------------------------------------





<div style="page-break-after: always;"></div>

<span id="TimeCountedOscilloscope"></span>

## TimeCountedOscilloscope

```C#
public class TimeCountedOscilloscope<T> : SingleThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ [SingleThreadOscilloscope](#SingleThreadOscilloscope)\<T\> $\rarr$ TimeCountedOscilloscope\<T\>
* interfaces: none
* summary:
  * the only difference with [SimpleOscilloscope](#SimpleOscilloscope) is the method [Draw](#TimeCountedOscilloscope\Draw)() will use a built-in watch to get delta-time.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SingleThreadOscilloscope](#SingleThreadOscilloscope\Constructor1)(ICanvas\<T\>, IPointDrawer,IGraphProducer,IControlPanel)||
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

<span id="MultiThreadOscilloscope"></span>

## MultiThreadOscilloscope

```C#
public abstract class MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ MultiThreadOscilloscope\<T\>
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
  |[MultiThreadOscilloscope](#MultiThreadOscilloscope\Constructor1)(ConstructorTuple\<ICanvas\<T\>\>, ConstructorTuple\<IPointDrawer\>, IGraphProducer, IControlPanel\[, ConcurrentQueue\<T\>=null\])||
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
  * ConcurrentQueue\<T\> buffer: the buffer of this oscilloscope. if null, a new ConcurrentQueue will be created as the buffer, and then you could get it with attribute [Buffer](#MultiThreadOscilloscope\Buffer).
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
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method. it should not be negative.
* Normal-Behaviour:
  * Pre-condition:
    * delta_time >= 0.
  * Post-Condition:
    * a new graph with type T will be produced and put into [Buffer](#MultiThreadOscilloscope\Buffer)




<div style="page-break-after: always;"></div>

<span id="UndrivedOscilloscope"></span>

## UndrivedOscilloscope

```C#
public class UndrivedOscilloscope<T> : MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ [MultiThreadOscilloscope](#MultiThreadOscilloscope)\<T\> $\rarr$ UndrivedOscilloscope\<T\>
* interfaces: none
* summary:
  * the only difference with [MultiThreadOscilloscope](#MultiThreadOscilloscope) is that the [Draw](#MultiThreadOscilloscope\Draw)() of [UndrivedOscilloscope](#UndrivedOscilloscope) is public.
* constructors:
  |name|describtion|
  |:-|:-|
  |[UndrivedOscilloscope](#UndrivedOscilloscope\Constructor1)(ConstructorTuple\<ICanvas\<T\>\>, ConstructorTuple\<IPointDrawer\>, IGraphProducer, IControlPanel\[, ConcurrentQueue\<T\>=null\])||
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
  * double delta_time: the time during which the point will be drawn on the graph. in short you'd better delivery the time span from the latest call of this method. it should not be negative.
* Normal-Behaviour:
  * Pre-condition:
    * delta_time >= 0.
  * Post-Condition:
    * a new graph with type T will be produced and put into [Buffer](#MultiThreadOscilloscope\Buffer)





<div style="page-break-after: always;"></div>

<span id="DrivedOscilloscope"></span>

## DrivedOscilloscope

```C#
public class DrivedOscilloscope<T> : MultiThreadOscilloscope<T>;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel)
* inheritance: Object $\rarr$ [MultiThreadOscilloscope](#MultiThreadOscilloscope)\<T\> $\rarr$ DrivedOscilloscope\<T\>
* interfaces: none
* summary:
  * a multi-thread oscilloscope that contains a built-in timer.
  * it will produce graphs periodically and put them into the [Buffer](#Multithreadoscilloscope\Buffer).
* constructors:
  |name|describtion|
  |:-|:-|
  |[DrivedOscilloscope](#DrivedOscilloscope\Constructor1)(ConstructorTuple\<ICanvas\<T\>\>, ConstructorTuple\<IPointDrawer\>, IGraphProducer, IControlPanel\[, ConcurrentQueue\<T\>=null\])||
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |bool|[IsRunning](#DrivedOscilloscope\IsRunning)|G|marks wheather this oscilloscope is running|
* methods:
  |name|describtion|
  |:-|:-|
  |void [Start](#DrivedOscilloscope\Start)(int)|start to produce graphs periodically.|
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
|interface|[IWave](#Wave\IWave)|describe a periodic wave with time, phase and voltage.|
|static class|[Waves](#Wave\Waves)|providing basics operations for IWave.|
|abstract class|[AbstractWave](#Wave\AbstractWave)|a better [IWave](#Wave\IWave) providing base operations for waves.|
|class|[FunctionWave](#Wave\FunctionWave)|a wave created with a $f_p(p)$.|
|class|[SinWave](#Wave\SinWave)|a wave described as $f_p(p)$ = max_voltage $\cdot \sin(2 \pi p)$.|
|class|[SawToothWave](#Wave\SawToothWave)|a wave described as $f_p(p)$ = max_voltage $\cdot(2p - 1)$.|
|class|[SquareWave](#Wave\SquareWave)|a wave described as $f_p(p)$ = max_voltage $\cdot$ (p \< $1 \over 2$ ? -max_voltage : max_voltage)|
|class|[ConstantWave](#Wave\ConstantWave)|a wave described as $f_p(p)$ = voltage. A DC wave.|
|class|[WaveFixer](#Wave\WaveFixer)|a mutable wave.|





<div style="page-break-after: always;"></div>

<span id="Wave\IWave"></span>

## IWave

```C#
    public interface IWave
    {
        double MeanVoltage { get; }

        int Period { get; }

        double Voltage(double phase);
    }
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* interfaces: none
* summary:
  * describe a periodic wave with time, phase and voltage.
* remarks
  * every object that implement this interface should be **immutable** object. 
  * if you want to change a wave, you can build a special class implementing IWave, whose constructor receive an IWave object as origin-wave. just like how [WaveReverser](#Wave\WaveReverser) do.
  * if you want a wave variable, you'd better not let it implement IWave. you could add a `GetStateShot()` method to return an IWave at certain time, just like how [WaveFixer](#Wave\WaveFixer) do.
  * this wave can be described with a function $f(t)$. the voltage at time $t$ is $f(t)$. $\exist S_T, s.t.$ $\forall T \in S_T,f(t) = f(t+T)$, then we define the period of this wave as $T = min(S_T)$, define the phase of this wave at time $t$ as $p={t \over T}$ ${\rm mod}$ $1$. in  `IWave`, we use [Period](#Wave\IWave\Period) to describe $T$ and use [Voltage](#Wave\IWave\Voltage)(double phase) to describe $f_p(p) = f(p \cdot T)$.
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\IWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\IWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\IWave\Voltage)(double)|return the voltage of this wave with certain phase|


### attributes:


<span id="Wave\IWave\MeanVoltage"></span>

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
  * the voltage at time $t$ is the same as the voltage at time $t + {\rm Period}$
* Getter
---------------------------------------------------------

### methods:

<span id="Wave\IWave\Voltage"></span>

```C#
double Voltage(double phase);
```

* Summary:
  * the voltage at certain phase.
* Params:
  * double phase: ${\rm phase} \in [0, 1)$. no exception will be raise if not, but it is still an undifined behavior.
* Return:
  * double: $f_p(p)=f(p\cdot T)$
* Normal-Behaviour:
  * Pre-Condition:
    * ${\rm phase} \in [0, 1)$
  * Post-Condition:
    * return $f_p(p)=f(p\cdot T)$
* Exception-Behaviour:
  * Exception null (no Exception will be throw out but this is undefined behavior):
    * phase \< 0 || phase >= 1
---------------------------------------------------------




<div style="page-break-after: always;"></div>

<span id="Wave\Waves"></span>

## Waves

```C#
public static class Waves
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ Waves
* interfaces: none
* summary:
  * a static class providing basics operations for IWave.
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |[ConstantWave](#Wave\ConstantWave)|[NONE](#Wave\Waves\NONE)|readonly|GND signal|
  |int|[UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND)|readonly|time-unit of this classlib is ${1 \over {\rm UNIT\_NUMBER\_PRO\_SECOND}} s$|
* methods:
  |name|describtion|
  |:-|:-|
  |double [GetFrequence](#Wave\Waves\GetFrequence)(IWave)|get the frequence of certain wave.|
  |double [CalculateMeanVoltage](#Wave\Waves\CalculateMeanVoltage)(IWave\[, int=1000\])|calculate the mean voltage of certain wave accoding to difination.|
  |[AbstractWave](#Wave\AbstractWave) [Add](#Wave\Waves\Add)(IWave,IWave)|add two wave, $g(t) = f_1(t) + f_2(t)$|
  |[AbstractWave](#Wave\AbstractWave) [Negative](#Wave\Waves\Negative)(IWave)|return a wave $g(t) = -f(t)$|
  |[AbstractWave](#Wave\AbstractWave) [Reverse](#Wave\Waves\Reverse)(IWave)|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|
  |[AbstractWave](#Wave\AbstractWave) [Decorate](#Wave\Waves\Decorate)(IWave)|decorate a [IWave](#Wave\IWave) as an [AbstractWave](#Wave\AbstractWave)|

### attributes:


<span id="Wave\Waves\NONE"></span>

```C#
public static readonly ConstantWave NONE = new ConstantWave(0);
```

* Summary:
  * GND signal
* readonly
---------------------------------------------------------


<span id="Wave\Waves\UNIT_NUMBER_PRO_SECOND"></span>

```C#
public static readonly int UNIT_NUMBER_PRO_SECOND = 1000_000;
```

* Summary:
  * time-unit of this classlib is ${1 \over {\rm UNIT\_NUMBER\_PRO\_SECOND}} s$
* Remarks
  * UNIT_NUMBER_PRO_SECOND = 1000_000 means the time-unit of this classlib is $\mu s$.
* readonly
---------------------------------------------------------



### methods:


<span id="Wave\Waves\GetFrequence"></span>

```C#
public static double GetFrequence(IWave wave);
```

* Summary:
  * get the frequence of certain wave.
* Remarks:
  * return [UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND) / (double)(wave.Period);
* Params:
  * IWave wave: the wave to calculate frequence.
* Return:
  * double: the frequence of this wave. frequence-unit is Hz.
* Normal-Behaviour:
  * Post-Condition:
    * return [UNIT_NUMBER_PRO_SECOND](#Wave\Waves\UNIT_NUMBER_PRO_SECOND) / (double)(wave.Period);
---------------------------------------------------------


<span id="Wave\Waves\CalculateMeanVoltage"></span>

```C#
public static double CalculateMeanVoltage(IWave wave, int calculate_times = 1000);
```

* Summary:
  * calculate the mean voltage of certain wave accoding to difination.
* Remarks:
  * this function is time-consuming, you'd better use `wave.MeanVoltage` to get the mean-voltage of wave if possible.
  * this function is mainly used to help the constructor of a wave calculating the mean-voltage. 
* Params:
  * [IWave](#Wave\IWave) wave: the wave that need to calculate mean_voltage.
  * int calculate_times: the bigger calcutate_times, the more precise the result will be, but the more time it will cost.
* Return:
  * double :${1 \over calculate\_times}\sum_{i=0}^{\rm calculate\_times} {\rm wave}.{\rm Voltage({i\over {\rm calculate\_times}})}$
* Normal-Behaviour:
  * Pre-Condition:
    * wave can be partly initialized, but make sure wave.[Voltage](#Wave\IWave\Voltage)() can work correctly.
  * Post-Condition
    * return ${1 \over calculate\_times}\sum_{i=0}^{\rm calculate\_times} {\rm wave}.{\rm Voltage({i\over {\rm calculate\_times}})}$
---------------------------------------------------------


<span id="Wave\Waves\Add"></span>

```C#
public static AbstractWave Add(IWave left, IWave right);
```

* Summary:
  * add two wave, $g(t) = f_1(t) + f_2(t)$.
* Remarks:
  * suggest we discribe left-wave by function $f_1(t)$, and right-wave by function $f_2(t)$, this function will return a new wave discribed by function $f_3(t)=f_1(t) + f_2(t)$.
  * the Period of the output wave will be the LCM (lowest common multiple) of the Period of each input wave.
* Params:
  * [IWave](#Wave\IWave) left: a wave that need to be add.
  * [IWave](#Wave\IWave) right: a wave that need to be add.
* Return:
  * [AbstractWave](#Wave\AbstractWave): a wave that observe the rules in Remarks.
---------------------------------------------------------


<span id="Wave\Waves\Negative"></span>

```C#
public static AbstractWave Negative(IWave origin);
```

* Summary:
  * return a wave $g(t) = -f(t)$.
* Params:
  * [IWave](#Wave\IWave) origin: origin wave;
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new AbstractWave;
* Normal-Behaviour:
  * Pre-Condition:
    * origin is an `immutable` object;
  * Post-Condition:
    * return AbstractWave new_wave;
    * new_wave.MeanVoltage + origin.MeanVoltage == 0;
    * new_wave.Period == origin.Period;
    * $\forall$ double p $\in [0, 1)$, new_wave.Voltage(p) + origin.Voltage(p) == 0;
---------------------------------------------------------


<span id="Wave\Waves\Reverse"></span>

```C#
public static AbstractWave Reverse(IWave origin);
```

* Summary:
  * reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$
* Params:
  * [IWave](#Wave\IWave) origin: origin wave;
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new AbstractWave;
* Normal-Behaviour:
  * Pre-Condition:
    * origin is an `immutable` object;
  * Post-Condition:
    * return AbstractWave new_wave;
    * new_wave.MeanVoltage == origin.MeanVoltage;
    * new_wave.Period == origin.Period;
    * $\forall$ double p $\in [0, 1)$, new_wave.Voltage(p) == origin.Voltage(1 - p);
---------------------------------------------------------


<span id="Wave\Waves\Decorate"></span>

```C#
public static AbstractWave Decorate(IWave origin);
```

* Summary:
  * decorate a [IWave](#Wave\IWave) as an [AbstractWave](#Wave\AbstractWave)
* Params:
  * [IWave](#Wave\IWave) origin: origin wave;
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new AbstractWave;
* Normal-Behaviour:
  * Pre-Condition:
    * origin is an `immutable` object;
  * Post-Condition:
    * return AbstractWave new_wave;
    * new_wave.MeanVoltage == origin.MeanVoltage;
    * new_wave.Period == origin.Period;
    * $\forall$ double p $\in [0, 1)$, new_wave.Voltage(p) == origin.Voltage(p);
---------------------------------------------------------


<div style="page-break-after: always;"></div>

<span id="Wave\AbstractWave"></span>

## AbstractWave

```C#
public abstract class AbstractWave : IWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ AbstractWave
* interfaces: [IWave](Wave\IWave)
* summary:
  * a better [IWave](#Wave\IWave) providing base operations for waves.
* remarks
  * Each `AbstractWave` should be an immutable object.
  * There is no fields in this class, so there is only default constructor.
  * The only reason why this class is designed is that, in .NET Standard 2.0, I cannot use C# 8.0, so I cannot add those operations to IWave derectly.
  * operator suntraction of 2 element is not provided, you can use `wave1 + (-wave2)` instead of `wave1 - wave2`, the latter is wrong.
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |abstract double|[MeanVoltage](#Wave\AbstractWave\MeanVoltage)|G|the mean voltage|
  |abstract int|[Period](#Wave\AbstractWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |abstract double [Voltage](#Wave\AbstractWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\AbstractWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|
* operators:
  |name|describtion|
  |:-|:-|
  |AbstractWave [Subtraction](#Wave\AbstractWave\Subtraction)(AbstractWave)|return a wave $g(t) = -f(t)$|
  |AbstractWave [Addition](#Wave\AbstractWave\Addition1)(AbstractWave, IWave)|add two wave, $g(t) = f_1(t) + f_2(t)$|
  |AbstractWave [Addition](#Wave\AbstractWave\Addition2)(IWave, AbstractWave)|add two wave, $g(t) = f_1(t) + f_2(t)$|

### attributes:


<span id="Wave\AbstractWave\MeanVoltage"></span>

```C#
public abstract double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\AbstractWave\Period"></span>

```C#
public abstract int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:



<span id="Wave\AbstractWave\Voltage"></span>

```C#
public abstract double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------


<span id="Wave\AbstractWave\Reverse"></span>

```C#
public AbstractWave Reverse();
```

* Summary:
  * reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$
* Remarks:
  * it behave the same as [Waves](#Wave\Waves).[Reverse](#Wave\Waves\Reverse)(this).
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new AbstractWave;
* Normal-Behaviour:
  * Post-Condition:
    * return AbstractWave new_wave;
    * new_wave.MeanVoltage == this.MeanVoltage
    * new_wave.Period == this.Period;
    * $\forall$ double p $\in [0, 1)$, new_wave.Voltage(p) == this.Voltage(1 - p);
---------------------------------------------------------


### operators:


<span id="Wave\AbstractWave\Subtraction"></span>

```C#
public static AbstractWave operator -(AbstractWave origin);
```

* Summary:
  * return a wave $g(t) = -f(t)$.
* Remarks:
  * it behave the save as [Waves](#Wave\Waves).[Negative](#Wave\Waves\Negative)(this).
* Params:
  * [IWave](#Wave\IWave) origin: origin wave;
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new AbstractWave;
* Normal-Behaviour:
  * Post-Condition:
    * return AbstractWave new_wave;
    * new_wave.MeanVoltage + origin.MeanVoltage == 0;
    * new_wave.Period == origin.Period;
    * $\forall$ double p $\in [0, 1)$, new_wave.Voltage(p) + origin.Voltage(p) == 0;
---------------------------------------------------------


<span id="Wave\AbstractWave\Addition1"></span>

```C#
public static AbstractWave operator +(AbstractWave left, IWave right);
```

* Summary:
  * add two wave, $g(t) = f_1(t) + f_2(t)$.
* Remarks:
  * it behave the save as [Waves](#Wave\Waves).[Add](#Wave\Waves\Add)(left, right).
  * suggest we discribe left-wave by function $f_1(t)$, and right-wave by function $f_2(t)$, this function will return a new wave discribed by function $f_3(t)=f_1(t) + f_2(t)$.
  * the Period of the output wave will be the LCM (lowest common multiple) of the Period of each input wave.
* Params:
  * [AbstractWave](#Wave\IWave) left: a wave that need to be add.
  * [IWave](#Wave\IWave) right: a wave that need to be add.
* Return:
  * [AbstractWave](#Wave\AbstractWave): a wave that observe the rules in Remarks.
---------------------------------------------------------


<span id="Wave\AbstractWave\Addition2"></span>

```C#
public static AbstractWave operator +(IWave left, AbstractWave right);
```

* Summary:
  * add two wave, $g(t) = f_1(t) + f_2(t)$.
* Remarks:
  * it behave the save as [Waves](#Wave\Waves).[Add](#Wave\Waves\Add)(left, right).
  * suggest we discribe left-wave by function $f_1(t)$, and right-wave by function $f_2(t)$, this function will return a new wave discribed by function $f_3(t)=f_1(t) + f_2(t)$.
  * the Period of the output wave will be the LCM (lowest common multiple) of the Period of each input wave.
* Params:
  * [IWave](#Wave\IWave) left: a wave that need to be add.
  * [AbstractWave](#Wave\IWave) right: a wave that need to be add.
* Return:
  * [AbstractWave](#Wave\AbstractWave): a wave that observe the rules in Remarks.
---------------------------------------------------------




<div style="page-break-after: always;"></div>

<span id="Wave\FunctionWave"></span>

## FunctionWave

```C#
public class FunctionWave : AbstractWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ [Abstractwave](#Wave\AbstractWave) $\rarr$ FunctionWave
* interfaces: [IWave](#Wave\IWave)
* summary:
  * a wave created with a $f_p(p)$.
* remarks
  * 
* delegates:
  |name|describtion|
  |:-|:-|
  |double [WaveFunction](#Wave\FunctionWave\WaveFunction)(double).|describtion of $f_p(p)$|
* constructors:
  |name|describtion|
  |:-|:-|
  |[FunctionWave](#Wave\FunctionWave\Constructor1)([WaveFunction](#Wave\FunctionWave\WaveFunction), int\[, double=1\])|create a FunctionWave and MeanVoltage will be calculated automatically.|
  |[FunctionWave](#Wave\FunctionWave\Constructor2)([WaveFunction](#Wave\FunctionWave\WaveFunction), int, double, double)|create a FunctionWave, using given MeanVoltage.|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\FunctionWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\FunctionWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\FunctionWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\FunctionWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|

### delegates

<span id="Wave\FunctionWave\WaveFunction"></span>

```C#
public delegate double WaveFunction(double phase);
```

* Summary:
  * describtion of $f_p(p)$
* Remark:
  * phase $\in [0, 1)$.


### constructors:


<span id="Wave\FunctionWave\Constructor1"></span>

```C#
public FunctionWave(WaveFunction function, int period, double voltage_times = 1);
```

* Summary:
  * create a FunctionWave and MeanVoltage will be calculated automatically.
* Remarks:
  * It may take some time to calculate the mean_voltage. If you want to make it faster, try to use another constructor.
* Params:
  * [WaveFunction](#Wave\FunctionWave\WaveFunction) function: the describtion of $f_p(p)$;
  * int period: the Period;
  * double voltage_times: this.Voltage(p) == voltage_times $\cdot$ function(p).
---------------------------------------------------------


<span id="Wave\FunctionWave\Constructor2"></span>

```C#
public FunctionWave(WaveFunction function, int period, double voltage_times, double function_mean);
```

* Summary:
  * Create a FunctionWave, using given MeanVoltage.
* Remarks:
  * Please make sure function_mean is correct. No check will be provided. function_mean == $\int_0^1 {\rm function}(p){\rm d}p$.
* Params:
  * [WaveFunction](#Wave\FunctionWave\WaveFunction) function: the describtion of $f_p(p)$;
  * int period: the Period;
  * double voltage_times: this.Voltage(p) == voltage_times $\cdot$ function(p).
  * double function_mean: the mean of param function, which means this.MeanVoltage == voltage_times $\cdot$ function_mean.
* Normal-Behaviour:
  * Pre-Condition:
    * function_mean == $\int_0^1 {\rm function}(p){\rm d}p$.
---------------------------------------------------------



### attributes:


<span id="Wave\FunctionWave\MeanVoltage"></span>

```C#
public double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\FunctionWave\Period"></span>

```C#
public int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:


<span id="Wave\FunctionWave\Voltage"></span>

```C#
public double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------



<span id="Wave\FunctionWave\Reverse"></span>

```C#
public AbstractWave Reverse()
```

* see also:
  * [Wave](#Wave).[AbstractWave](#Wave\AbstractWave).[Reverse](#Wave\AbstractWave\Reverse)().
---------------------------------------------------------



<div style="page-break-after: always;"></div>

<span id="Wave\SinWave"></span>

## SinWave

```C#
public class SinWave : FunctionWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ [Abstractwave](#Wave\AbstractWave) $\rarr$ [FinctionWave](#Wave\FunctionWave) $\rarr$ SinWave
* interfaces: [IWave](#Wave\IWave)
* summary:
  * a wave described as $f_p(p)$ = max_voltage $\cdot \sin(2 \pi p)$
* remarks
  * just like [FunctionWave](#Wave\FunctionWave)(phase => Math.Sin(2 * Math.PI * phase), period, max_voltage, 0);
* constructors:
  |name|describtion|
  |:-|:-|
  |[SinWave](#Wave\SinWave\Constructor1)(int, double)|create a sin-wave with given period and max_voltage|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\SinWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\SinWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\SinWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\SinWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|

### constructors:


<span id="Wave\SinWave\Constructor1"></span>

```C#
public SinWave(int period, double max_voltage);
```

* Summary:
  * create a sin-wave with given period and max_voltage.
* Params:
  * int period: the Period of this wave.
  * double max_voltage: the max voltage of this wave. In other way, $f_p({1 \over 4})=$ max_voltage.
---------------------------------------------------------

### attributes:


<span id="Wave\SinWave\MeanVoltage"></span>

```C#
public double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\SinWave\Period"></span>

```C#
public int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:


<span id="Wave\SinWave\Voltage"></span>

```C#
public double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------



<span id="Wave\SinWave\Reverse"></span>

```C#
public AbstractWave Reverse()
```

* see also:
  * [Wave](#Wave).[AbstractWave](#Wave\AbstractWave).[Reverse](#Wave\AbstractWave\Reverse)().
---------------------------------------------------------





<div style="page-break-after: always;"></div>

<span id="Wave\SawToothWave"></span>

## SawToothWave

```C#
public class SawToothWave : FunctionWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ [Abstractwave](#Wave\AbstractWave) $\rarr$ [FinctionWave](#Wave\FunctionWave) $\rarr$ SawToothWave
* interfaces: [IWave](#Wave\IWave)
* summary:
  * a wave described as $f_p(p)$ = max_voltage $\cdot(2p - 1)$.
* remarks
  * just like [FunctionWave](#Wave\FunctionWave)(phase => 2 * phase - 1, period, max_voltage, 0);
* constructors:
  |name|describtion|
  |:-|:-|
  |[SawToothWave](#Wave\SawToothWave\Constructor1)(int, double)|create a sin-wave with given period and max_voltage|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\SawToothWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\SawToothWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\SawToothWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\SawToothWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|

### constructors:


<span id="Wave\SawToothWave\Constructor1"></span>

```C#
public SawToothWave(int period, double max_voltage);
```

* Summary:
  * create a sin-wave with given period and max_voltage.
* Params:
  * int period: the Period of this wave.
  * double max_voltage: the max voltage of this wave. In other way, $\forall p \in [0, 1), f_p(p)=$ max_voltage $\cdot(2p - 1)$.
---------------------------------------------------------

### attributes:


<span id="Wave\SawToothWave\MeanVoltage"></span>

```C#
public double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\SawToothWave\Period"></span>

```C#
public int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:


<span id="Wave\SawToothWave\Voltage"></span>

```C#
public double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------



<span id="Wave\SawToothWave\Reverse"></span>

```C#
public AbstractWave Reverse()
```

* see also:
  * [Wave](#Wave).[AbstractWave](#Wave\AbstractWave).[Reverse](#Wave\AbstractWave\Reverse)().
---------------------------------------------------------




<div style="page-break-after: always;"></div>

<span id="Wave\SquareWave"></span>

## SquareWave

```C#
public class SquareWave : FunctionWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ [Abstractwave](#Wave\AbstractWave) $\rarr$ [FinctionWave](#Wave\FunctionWave) $\rarr$ SquareWave
* interfaces: [IWave](#Wave\IWave)
* summary:
  * a wave described as $f_p(p)$ = max_voltage $\cdot$ (p \< $1 \over 2$ ? -max_voltage : max_voltage)
* remarks
  * just like [FunctionWave](#Wave\FunctionWave)(phase => phase < 0.5 ? -1 : 1, period, max_voltage, 0);
* constructors:
  |name|describtion|
  |:-|:-|
  |[SquareWave](#Wave\SquareWave\Constructor1)(int, double)|create a sin-wave with given period and max_voltage|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\SquareWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\SquareWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\SquareWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\SquareWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|

### constructors:


<span id="Wave\SquareWave\Constructor1"></span>

```C#
public SquareWave(int period, double max_voltage);
```

* Summary:
  * create a sin-wave with given period and max_voltage.
* Params:
  * int period: the Period of this wave.
  * double max_voltage: the max voltage of this wave. In other way, $\forall p \in [0, {1\over 2}), f_p(p)=$ -max_voltage, $\forall p \in [{1\over 2}, 1), f_p(p)=$ max_voltage.
---------------------------------------------------------

### attributes:


<span id="Wave\SquareWave\MeanVoltage"></span>

```C#
public double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\SquareWave\Period"></span>

```C#
public int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:


<span id="Wave\SquareWave\Voltage"></span>

```C#
public double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------



<span id="Wave\SquareWave\Reverse"></span>

```C#
public AbstractWave Reverse()
```

* see also:
  * [Wave](#Wave).[AbstractWave](#Wave\AbstractWave).[Reverse](#Wave\AbstractWave\Reverse)().
---------------------------------------------------------






<div style="page-break-after: always;"></div>

<span id="Wave\ConstantWave"></span>

## ConstantWave

```C#
public class ConstantWave : AbstractWave
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[Wave](#Wave)
* inheritance: Object $\rarr$ [Abstractwave](#Wave\AbstractWave) $\rarr$ ConstantWave
* interfaces: [IWave](#Wave\IWave)
* summary:
  * a wave described as $f_p(p)$ = voltage.
  * In other way, it is a DC wave.
* remarks
  * just like [FunctionWave](#Wave\FunctionWave)(phase => 1, 1, voltage, voltage);
* constructors:
  |name|describtion|
  |:-|:-|
  |[ConstantWave](#Wave\ConstantWave\Constructor1)(double)|create a DC-wave with given voltage|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[MeanVoltage](#Wave\ConstantWave\MeanVoltage)|G|the mean voltage|
  |int|[Period](#Wave\ConstantWave\Period)|G|the period of this wave|
* methods:
  |name|describtion|
  |:-|:-|
  |double [Voltage](#Wave\ConstantWave\Voltage)(double)|return the voltage of this wave with certain phase|
  |AbstractWave [Reverse](#Wave\ConstantWave\Reverse)()|reverse the phase of a wave, $g(t) = g_p({t \over T} {\rm mod}$ $1) = f_p(1 - ({t \over T} {\rm mod}$ $1)) = f(T - t)$|

### constructors:


<span id="Wave\ConstantWave\Constructor1"></span>

```C#
public ConstantWave(int period, double max_voltage);
```

* Summary:
  * create a DC-wave with given voltage.
* Params:
  * double voltage: the voltage of this wave. In other way, $\forall p \in [0, 1), f_p(p)=$ voltage.
---------------------------------------------------------

### attributes:


<span id="Wave\ConstantWave\MeanVoltage"></span>

```C#
public double MeanVoltage { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[MeanVoltage](#Wave\IWave\MeanVoltage).
---------------------------------------------------------


<span id="Wave\ConstantWave\Period"></span>

```C#
public int Period { get; }
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Period](#Wave\IWave\MeanVoltage).
---------------------------------------------------------

### methods:


<span id="Wave\ConstantWave\Voltage"></span>

```C#
public double Voltage(double phase);
```

* see also:
  * [Wave](#Wave).[IWave](#Wave\IWave).[Voltage](#Wave\IWave\Voltage)().
---------------------------------------------------------



<span id="Wave\ConstantWave\Reverse"></span>

```C#
public AbstractWave Reverse()
```

* see also:
  * [Wave](#Wave).[AbstractWave](#Wave\AbstractWave).[Reverse](#Wave\AbstractWave\Reverse)().
---------------------------------------------------------





<div style="page-break-after: always;"></div>

<span id="Wave\WaveFixer"></span>

## WaveFixer

```C#
public class WaveFixer
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[](#)
* inheritance: Object $\rarr$ WaveFixer
* interfaces: none
* summary:
  * a mutable wave.
* remarks
  * use [GetStateShot](#Wave\WaveFixer\GetStateShot)() to get an [AbstractWave](#Wave\AbstractWave) as the shot of fixed wave now.
* constructors:
  |name|describtion|
  |:-|:-|
  |[WaveFixer](#Wave\WaveFixer\Constructor1)()|create a WaveFixer with GND wave|
  |[WaveFixer](#Wave\WaveFixer\Constructor2)([IWave](#Wave\IWave))|create a WaveFixer with given wave|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |double|[VoltageTimes](#Wave\WaveFixer\VoltageTimes)|GS|the times of voltage|
  |double|[PeriodTimes](#Wave\WaveFixer\PeriodTimes)|GS|the times of period|
  |[IWave](#Wave\IWave)|[Wave](#Wave\WaveFixer\Wave)|GS|the base wave|
* methods:
  |name|describtion|
  |:-|:-|
  |[AbstractWave](Wave\AbstractWave) [GetStateShot](#Wave\WaveFixer\GetStateShot)()|get the shot of the fixed wave now|

### constructors:


<span id="Wave\WaveFixer\Constructor1"></span>

```C#
public WaveFixer();
```

* Summary:
  * create a WaveFixer with GND wave. 
* Remarks:
  * the same as [WaveFixer](#Wave\WaveFixer\Constructor2)([Waves](#Wave\Waves).[NONE](#Wave\Waves\NONE)).
---------------------------------------------------------


<span id="Wave\WaveFixer\Constructor2"></span>

```C#
public WaveFixer(IWave wave);
```

* Summary:
  * create a WaveFixer with given wave
* Params:
  * [IWave](#Wave\IWave) wave: given wave that this WaveFixer will use.
---------------------------------------------------------

### attributes:


<span id="Wave\WaveFixer\VoltageTimes"></span>

```C#
public double VoltageTimes { get; set; }
```

* Summary:
  * the Voltage of fixed wave at phase p is Wave.Voltage(p) $\cdot$ VoltageTimes.
* Invarient:
  * $\forall p \in [0, 1)$, [GetStateShot](#Wave\WaveFixer\GetStateShot)().Voltage(p) == Wave.Period * VoltageTimes.
* Getter
* Setter
---------------------------------------------------------


<span id="Wave\WaveFixer\PeriodTimes"></span>

```C#
public double PeriodTimes { get; set; }
```

* Summary:
  * the Period of fixed wave is (int)(Wave.Period $\cdot$ PeriodTimes).
* Invarient:
  * [GetStateShot](#Wave\WaveFixer\GetStateShot)().Period == (int)(Wave.Period * PeriodTimes)
* Getter
* Setter
---------------------------------------------------------


<span id="Wave\WaveFixer\Wave"></span>

```C#
public IWave Wave { get; set; }
```

* Summary:
  * the base wave.
* Invarient:
  * [GetStateShot](#Wave\WaveFixer\GetStateShot)().Period == (int)(Wave.Period * PeriodTimes)
  * $\forall p \in [0, 1)$, [GetStateShot](#Wave\WaveFixer\GetStateShot)().Voltage(p) == Wave.Period * VoltageTimes.
* Getter
* Setter:
  * if value is null, Wave will be set to [Waves](#Wave\Waves).[NONE](#Wave\Waves\NONE).
---------------------------------------------------------

### methods:



<span id="Wave\WaveFixer\GetStateShot"></span>

```C#
public AbstractWave GetStateShot();
```

* Summary:
  * get the shot of the fixed wave now.
* Return:
  * [AbstractWave](#Wave\AbstractWave): a new wave that can describe the wave now.
* Normal-Behaviour:
  * Post-Condition:
    * AbstractWave new_wave;
    * new_wave.Period == (int)(Wave.Period * PeriodTimes)
    * $\forall p \in [0, 1)$, new_wave.Voltage(p) == Wave.Period * VoltageTimes.
    * return new_wave.
---------------------------------------------------------




 









<div style="page-break-after: always;"></div>

<span id="Tools"></span>

## Tools

```C#
namespace OscilloscopeKernel.Tools
```

Summary:
* 

|type|name|description|
|:-|:-|:-|
|readonly struct|[SizeStruct](#Tools/SizeStruct)|a struct to describe a size of something.|
||[](#)||
||[](#)||
||[](#)||
||[](#)||
||[](#)||






<div style="page-break-after: always;"></div>

<span id="#Tools/SizeStruct"></span>

## SizeStruct

```C#
public readonly struct SizeStruct;
```

* namespace: [OscilloscopeKernel](#OscilloscopeKernel).[](#)
* inheritance: none
* interfaces: none
* summary:
  * a struct to describe a size of something.
* remarks
  * it is a readonly stract, every attribute of it is unchangable.
* constructors:
  |name|describtion|
  |:-|:-|
  |[SizeStruct](#Tools\SizeStruct\Constructor1)(int, int)|create a SizeStruct with given length and width.|
* attributes:
  |type|name|accessor|describtion|
  |:-|:-|:-|:-|
  |int|[Length](#Tools\SizeStruct\Length)|G|the length.|
  |int|[Width](#Tools\SizeStruct\Width)|G|the width.|
* operators:
  |name|describtion|
  |:-|:-|
  |[Euqal](#Tools\SizeStruct\Equal)(SizeStruct, SizeStruct)||
  |[NotQueal](#Tools\SizeStruct\NotEqual)(SizeStruct, SizeStruct)||

### constructors:


<span id="Tools\SizeStruct\Constructor1"></span>

```C#
public SizeStruct(int length, int width);
```

* Summary:
  * create a struct to describe a 2D size with length and width.
* Params:
  * int length: the length.
  * int width: the width.
* Normal-Behaviour:
  * Post-Condition:
    * Width == width.
    * Length == length.
---------------------------------------------------------

### attributes:


<span id="Tools\SizeStruct\Length"></span>

```C#
public int Length { get; }
```

* Getter:
---------------------------------------------------------


<span id="Tools\SizeStruct\Width"></span>

```C#
public int Width { get; }
```

* Getter:
---------------------------------------------------------

### operators:


<span id="Tools\SizeStruct\Equal"></span>

```C#
public static bool operator ==(SizeStruct left, SizeStruct right);
```

* Summary:
  * compare 2 SizeStruct, return true only if both Width and Length of the two SizeStruct are the same.
* Params:
  * SizeStruct left: a SizeStruct need to be compared.
  * SizeStruct right: a SizeStruct need to be compared.
* Return:
  * bool: (left.Length == right.Length) && (left.Width == right.Width)
* Normal-Behaviour:
  * Post-Condition:
    * \result == (left.Length == right.Length) && (left.Width == right.Width)
    * SizeStruct A,B,C, A==B && B==C => A==C
---------------------------------------------------------


<span id="Tools\SizeStruct\NotEqual"></span>

```C#
public static bool operator !=(SizeStruct left, SizeStruct right);
```

* Summary:
  * compare 2 SizeStruct, return true if any Width or Length of the two SizeStruct are the same.
* Params:
  * SizeStruct left: a SizeStruct need to be compared.
  * SizeStruct right: a SizeStruct need to be compared.
* Return:
  * bool: (left.Length != right.Length) || (left.Width != right.Width);
* Normal-Behaviour:
  * Post-Condition:
    * \result == (left.Length == right.Length) && (left.Width == right.Width)
    * SizeStruct A,B, (A!=B) == !(A==B)
---------------------------------------------------------












































<div style="page-break-after: always;"></div>

<span id="Drawing"></span>

## Drawing

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

<span id="Producer"></span>

## Producer

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

<span id="Exceptions"></span>

## Exceptions

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

<span id="OscilloscopeFramework"></span>

## OscilloscopeFramework

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



