var comp = app.project.activeItem;
if (comp && comp instanceof CompItem) {
   var rocket = comp.layer('rocket');
   var shape = comp.layer('flightPath').content('Shape 1');
   var path = shape.content('Path 1').path;
   $.writeln('Start...');
   $.writeln(path.name);
   for (var i = 1; i <= path.numProperties; i++) {
      $.writeln('\t' + path.property(i).name);
   }
   if (rocket && path) {
      rocket.autoOrient = AutoOrientType.NO_AUTO_ORIENT;
      pos = path.pointOnPath(percentage = 0.5, t = time);// t / thisComp.duration);
      //tng = path.tangentOnPath(t / thisComp.duration);
      $.writeln(pos);
      $.writeln(tng);
   } else {
      $.writeln('Layers not found!');
   }
} else {
   $.writeln('No active composition found!');
}


// INTERNAL SYNTAX
var t = time / 10;//thisComp.duration;
var flightPath = thisComp.layer('flightPath');
var shape = flightPath.content('Shape 1');
var path = shape.content('Path 1').path;
path.pointOnPath(t, 0) + flightPath.transform.position;
//throw (path.tangentOnPath(t, 0));
//transform.position