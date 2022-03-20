
using System;
using System.Collections.Generic;
using System.Linq;

namespace factorized.utility {
    public class product : IEquatable<product>{
        private Dictionary<int,int> _items = new ();
        private Dictionary<string,double> _values  = new ();
        private Dictionary<string,string> _properties = new();
        public Dictionary<int,int> items {
            get { return _items;} 
            set{_items = new(value,value.Comparer);}}
        public Dictionary<string, double> values{
            get{return _values;} 
            set{_values = new (value,value.Comparer);}}
        public Dictionary<string,string> properties{
            get{return _properties;} 
            set{_properties = new (value,value.Comparer);}}
        
        public product(){
            items = new ();
            values = new ();
            properties = new ();
        }
        
        public product(product orig){
            items =  new (orig.items, orig.items.Comparer);
            values =  new (orig.values, orig.values.Comparer);
            properties = new (orig.properties, orig.properties);
        }

        public bool Equals(product other)
        {
            bool overallEquality = true;
            if(items.Comparer.GetType() == other?.items.Comparer.GetType() && values.Comparer.GetType() == other?.values.Comparer.GetType() 
            && properties.Comparer.GetType() == other?.properties.Comparer.GetType()){
                overallEquality = overallEquality && items.Keys.ToHashSet(items.Comparer).SetEquals(other.items.Keys);
                overallEquality = overallEquality && values.Keys.ToHashSet(values.Comparer).SetEquals(other.values.Keys);
                overallEquality = overallEquality && properties.Keys.ToHashSet(properties.Comparer).SetEquals(other.properties.Keys);
                return overallEquality;
            }
            else{
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if(this.GetType() == obj?.GetType()){
                return Equals((product)obj);
            }
            else{
                return false;
            }
        }

        public override int GetHashCode()
        {
            return items.GetHashCode() | values.GetHashCode() | properties.GetHashCode();
        }

        public override string ToString()
        {
            string message = "\nitems: ";
            foreach(KeyValuePair<int,int>item in items){
                message += "\nitem type:"+item.Key.ToString() + " item stack:" + item.Value.ToString() ;
            }
            message +="\n\n values:";
            foreach(KeyValuePair<string,double> value in values){
                message += "\n" + value.Key +": " +value.Value.ToString(); 
            }
            message += "\n\n properties: ";
            foreach(KeyValuePair<string,string> property in properties){
                message += "\n" +property.Key +": "+property.Value;
            }
            return message;
        }
        public static bool operator ==(product obj1, product obj2){
            return obj1.Equals(obj2);
        }
        public static bool operator !=(product obj1, product obj2){
            return !(obj1 == obj2);
        }
    }
}
