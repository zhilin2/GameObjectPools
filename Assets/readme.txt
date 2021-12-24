使用规则：
1.对需要回收的预制挂上RecycleComponent组件。
2.需要将项目中使用“Instantiate”，“Destroy”方法替换为“MyObject.Instantiate”，“MyObject.Destroy”。
3.RecycleComponent组件的“tag”，如果填预制路径，在实例化的时候可以使用路径方式，可以修改UIPoolManager的52行加载方式。
4.RecycleComponent组件的“tag”必须唯一。
5.必须准时对象池使用规则：
	不能改变gameObject结构，比如删除某些节点或者加一些节点后删除。
	不能对删除的gameObject做处理，（比如多次删除，挂上定时器执行某些操作）