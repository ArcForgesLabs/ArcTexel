using ArcTexel.Models.Handlers;

namespace ArcTexel.Models.Events;

public delegate void NodePropertyValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args);

public record NodePropertyValueChangedArgs(object? OldValue, object? NewValue);
