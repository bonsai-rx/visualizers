# Getting Started

`Bonsai.Gui` is a collection of packages for the [Bonsai](https://bonsai-rx.org/) visual programming language for quickly composing user interfaces and real-time data visualization.

To install `Bonsai.Gui` use the Bonsai package manager and search for the **Bonsai - GUI** package.

## Build user interfaces

`Bonsai.Gui` includes a collection of several common user interface controls, including buttons, sliders, combo boxes, text boxes, etc.

:::workflow
![Basic Controls](~/workflows/basic_controls.bonsai)
:::

At runtime, each control operator exposes its own visualizer displaying only that specific control. To combine multiple controls together, we use container controls, such as group boxes, tab controls, or table layout panels.

:::workflow
![Container Controls](~/workflows/container_controls.bonsai)
:::

To add controls into the container, we use the [`VisualizerMapping`](xref:Bonsai.Expressions.VisualizerMappingBuilder) operator to hierarchically overlay the controls.

## Compose real-time visualizations

`Bonsai.Gui.ZedGraph` provides a framework for assembling complex real-time reactive plots using bar graphs, line graphs or rolling graphs.

:::workflow
![Basic Graphs](~/workflows/basic_graphs.bonsai)
:::

Similar to GUI elements, each of these graphs exposes a visualizer, where updates are synchronized and driven by data arriving from the input sequence. Multiple traces or bars can be plotted in the same graph if the input values are complex data types.

Multiple independent graph visualizers can also be combined and updated asynchronously using graph panels.

:::workflow
![Graph Panels](~/workflows/graph_panels.bonsai)
:::

Similar to GUI elements, we can add graphs into a graph panel by using the [`VisualizerMapping`](xref:Bonsai.Expressions.VisualizerMappingBuilder) operator. This can be used for example to create visualizations where part of the plot is made up of continuous periodic traces, and other parts are overlaid dynamically using sparse or infrequent updates from other data sequences.

## Example

The following example builds a simple GUI with a slider and a button, where values of the slider are plotted continuously on a graph. A red dot is overlaid marking the current value of the slider every time the button is clicked.

:::workflow
![Example](~/workflows/gui-demo.bonsai)
:::