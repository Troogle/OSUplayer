using System;
using System.ComponentModel;
using System.Drawing;
using Telerik.WinControls;
using Telerik.WinControls.Layouts;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace OSU_player
{
    public class BubbleBar : RadControl
    {
        BubbleBarElement element;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BubbleBarElement Element { get { return this.element; } }
        [RadEditItemsAction]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RadItemOwnerCollection Items { get { return this.element.Items; } }
        protected override void CreateChildItems(RadElement parent)
        {
            this.element = new BubbleBarElement();
            this.element.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            parent.Children.Add(this.element);
        }
    }
    public class BubbleBarElement : RadElement
    {
        FillPrimitive fill;
        BorderPrimitive border;
        StackLayoutPanel panel;
        RadItemOwnerCollection items;
        public BubbleBarElement() { }
        protected override void InitializeFields()
        {
            base.InitializeFields();
            this.Shape = new RoundRectShape(12);
            this.items = new RadItemOwnerCollection();
            this.items.ItemTypes = new Type[] { typeof(RadButtonElement) };
            this.items.ItemsChanged += new ItemChangedDelegate(items_ItemsChanged);
        }
        public RadItemOwnerCollection Items { get { return items; } }
        protected override void CreateChildElements()
        {
            fill = new FillPrimitive();
            fill.BackColor = Color.FromArgb(253, 253, 253);
            fill.BackColor2 = Color.FromArgb(112, 112, 112);
            fill.NumberOfColors = 2;
            fill.GradientStyle = GradientStyles.Linear;
            fill.GradientAngle = 90;
            fill.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            this.Children.Add(fill);
            border = new BorderPrimitive();
            border.GradientStyle = GradientStyles.Solid;
            border.ForeColor = Color.FromArgb(0, 0, 0);
            border.AutoSizeMode = RadAutoSizeMode.FitToAvailableSize;
            this.Children.Add(border);
            panel = new StackLayoutPanel();
            panel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //panel.Margin = new System.Windows.Forms.Padding(0, 20, 10, 0); 
            panel.Alignment = ContentAlignment.MiddleCenter;
            panel.StretchHorizontally = false;
            this.Children.Add(panel);
            this.items.Owner = panel;
        }
        void items_ItemsChanged(RadItemCollection changed, RadItem target, ItemsChangeOperation operation)
        {
            if (operation == ItemsChangeOperation.Inserted || operation == ItemsChangeOperation.Set)
            {
                target.AddBehavior(new BubbleBarMouseOverBehavior());
            }
        }
    }
    public class BubbleBarMouseOverBehavior : PropertyChangeBehavior
    {
        public BubbleBarMouseOverBehavior() : base(RadItem.IsMouseOverProperty) { }
        public override void OnPropertyChange(RadElement element, RadPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                element.ResetValue(RadElement.ScaleTransformProperty);
                AnimatedPropertySetting animatedExpand = new AnimatedPropertySetting(RadElement.ScaleTransformProperty, new SizeF(0.65f, 0.65f), new SizeF(1f, 1f), 5, 30); animatedExpand.ApplyValue(element);
            }
            else
            {
                AnimatedPropertySetting animatedExpand = new AnimatedPropertySetting(RadElement.ScaleTransformProperty, null, new SizeF(0.65f, 0.65f), 5, 30);
                animatedExpand.ApplyValue(element);
            }
        }
    }
}
