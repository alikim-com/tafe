
function Expression_SemaphoreTrackers() {

   var keyStart = key(2).time;
   if (time < keyStart) {
      transform.position
   } else {
      var tracker = thisComp.layer("telescope").transform.position;
      var dif = tracker - tracker.valueAtTime(keyStart);
      transform.position + dif;
   }


   var keyStart = key(2).time;
   if (time <= keyStart) {
      thisProperty;
   } else {
      var tel = thisComp.layer("telescope");
      var telScl = tel.transform.scale;
      var telSclStart = telScl.valueAtTime(keyStart);
      var mult = [telScl[0] / telSclStart[0], telScl[1] / telSclStart[1]];
      var telPos = tel.transform.position;
      var telPosStart = telPos.valueAtTime(keyStart);
      var difStart = (thisLayer.transform.position + thisProperty.valueAtTime(keyStart)) - telPosStart;
      [telPos[0] + difStart[0] * mult[0], telPos[1] + difStart[1] * mult[1]] - thisLayer.transform.position;
   }

   var keyStart = content("Rectangle 1").transform.position.key(2).time;
   if (time <= keyStart) {
      thisProperty;
   } else {
      var tel = thisComp.layer("telescope");
      var telScl = tel.transform.scale;
      var telSclStart = telScl.valueAtTime(keyStart);
      var mult = [telScl[0] / telSclStart[0], telScl[1] / telSclStart[1]];
      [thisProperty[0] * mult[0], thisProperty[1] * mult[1]];
   }

}


function MoveShapeGroupsToLayers() {
   var comp = app.project.activeItem;
   if (!comp || !(comp instanceof CompItem)) {
      $.writeln("Please select a composition");
      return;
   }

   var sourceLayer = comp.selectedLayers[0];
   if (!(sourceLayer instanceof ShapeLayer)) {
      $.writeln("Selected layer must be shape layer");
      return;
   }

   var sourceLayerContent = sourceLayer.content;
   var sourceContentLength = sourceLayerContent.numProperties;

   for (var i = 1; i <= sourceContentLength; i++) {
      var sourceLayerGroup = sourceLayerContent.property(i);
      var sourceLayerGroupName = sourceLayerGroup.name;
      var newLayer = sourceLayer.duplicate();
      newLayer.name = sourceLayer.name + ' ' + sourceLayerGroupName;

      var newLayerContent = newLayer.content;
      for (var j = 1; j <= newLayerContent.numProperties; j++) {
         var newLayerGroup = newLayerContent.property(j);
         if (newLayerGroup.name != sourceLayerGroupName) {
            newLayerGroup.remove();
            j--;
         }
      }
   }

}

function CopyProperties(sourceLayerName, destLayers) {
   var comp = app.project.activeItem;
   if (!comp || !(comp instanceof CompItem)) {
      $.writeln('Please select a composition');
      return;
   }

   var sourceLayer = comp.layer(sourceLayerName);
   if (!sourceLayer) {
      $.writeln('Layer "' + sourceLayerName + '" not found');
      return;
   }

   var propsGroup = 'materialOption';
   var propsToCopy = [
      'ambient',
      'diffuse',
      'specularIntensity',
      'specularShininess',
      'metal',
      'reflectionIntensity',
      'reflectionSharpness',
      'reflectionRolloff'
   ];

   for (var s = 0; s < destLayers.length; s++) {

      var destLayer = comp.layer(destLayers[s]);
      if (!destLayer) {
         $.writeln('Layer "' + destLayer.name + '" not found');
         continue;
      }

      // access instance
      // thisComp.layer("rocket").source.layer("rocket").materialOption.ambient
      // access class
      // comp("rocket").layer("rocket").materialOption.ambient

      // var instanceLayer = 'thisComp.layer("' + sourceLayerName + '").source.layer("' + sourceLayerName + '")';
      var instanceLayer = 'thisComp.layer("' + sourceLayerName + '")';

      for (var i = 0; i < propsToCopy.length; i++) {
         var propName = propsToCopy[i];
         var expression = instanceLayer + '.' + propsGroup + '.' + propName;
         destLayer.property(propName).expression = expression;
      }

      // fix extrusion
      var propName2 = 'extrusionDepth';
      var propsGroup2 = 'geometryOption';
      var expression2 = instanceLayer + '.' + propsGroup2 + '.' + propName2;
      destLayer.geometryOption.property(propName2).expression = expression2;
   }

}

//MoveShapeGroupsToLayers();

CopyProperties(
   'rocket',
   [
      'star 1',
      'star 2',
      'star 3',
      'sun 1',
      'sun 2',
      'sun 3',
      'sun 4',
      'ring',
   ]);