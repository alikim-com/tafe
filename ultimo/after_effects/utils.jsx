
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

// ------------------

function includes(arr, item) {
   for (var i = 0; i < arr.length; i++)
      if (arr[i] == item) return true;
   return false;
}

function GetProject() {
   var _proj = app.project;
   var msg = _proj ? 'project found' : 'no project found';
   $.writeln(msg);
   return _proj;
}

function GetItemByName(collection, itemName) {
   if (!collection) {
      $.writeln( 'collection is empty');
      return null;
   }
   var item = null;
   var itemFound = false;
   for (var j = 0; j < collection.items.length; j++) {
      item = collection.item(j + 1);
      if (item.name != itemName) continue;
      itemFound = true;
      break;
   }
   if (!itemFound) {
      $.writeln('item "' + itemName + '" not found in "' + collection.name + '"');
      return null;
   }
   return item;
}

function GetFolderByName(collection, folderName) {
   var folder = null;
   var folderFound = false;
   for (var j = 0; j < collection.items.length; j++) {
      folder = collection.item(j + 1);
      if (!(folder instanceof FolderItem) || folder.name != folderName) continue;
      folderFound = true;
      break;
   }
   if (!folderFound) {
      $.writeln('folder "' + folderName + '" not found in collection "' + collection.name + '"');
      return null;
   }
   return folder;
}

function IsFootage(item, verbose) {
   if (!item) return false;
   if (!(item instanceof FootageItem)) {
      if(verbose) $.writeln(item.name + ' is not a footage item');
      return false;
   }
   return true;
}

function IsComp(item, verbose) {
   if (!item) return false;
   if (!(item instanceof CompItem)) {
      if(verbose) $.writeln(item.name + ' is not a composition');
      return false;
   }
   return true;
}

function GetActiveComp(proj) {
   return IsComp(proj.activeItem) ? proj.activeItem : null;
}

function GetSelectedLayer(comp) {
   var layer = comp.selectedLayers[0];
   if (!layer) {
      msg = 'no layer selected';
      $.writeln(msg);
   }
   return layer;
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

function ResizeToDuration() {
   var proj = GetProject();
   if (!proj) return;
   var comp = GetActiveComp(proj);
   if (!comp) return;
   var layer = GetSelectedLayer(comp);
   if (!layer) return;
   var src = layer.source;
   if (!src || !(src instanceof CompItem)) return;
   layer.inPoint = layer.startTime;
   layer.outPoint = layer.startTime + src.duration;
}

function SwapFootageItemsByName(replacementFolderName, layerNames) {
   // replaces footage inside comps with (highres) version
   // from replacementFolder (same file name)

   const proj = GetProject();
   if (!proj) return;
   const comp = GetActiveComp(proj);
   if (!comp) return;

   var folder = GetFolderByName(proj, replacementFolderName);
   if (!folder) return;

   var layer = null;
   var src = null;
   var repItem = null;
   for (var i = 0; i < layerNames.length; i++) {
      layer = comp.layer(layerNames[i]);
      if (!layer) {
         $.writeln('layer"' + layerNames[i] + '" not found');
         continue;
      }
      src = layer.source;
      if (src == null || !(src instanceof CompItem)) {
         $.writeln('comp "' + layerNames[i] + '" is not a proper comp');
         continue;
      }
      layer = src.layer(1);
      if (!layer) {
         $.writeln('comp "' + layerNames[i] + '" is empty');
         continue;
      }
      src = layer.source;
      if (src == null || !(src instanceof FootageItem)) {
         continue;
      }
      repItem = GetItemByName(folder, src.name);
      if (!repItem) {
         $.writeln('replacement for "' + src.name + '" not found');
         continue;
      }
      layer.replaceSource(repItem, false);
      layer.transform.scale.setValue([100, 100]);
      break;
   }
}

function SwapFootageItems(filesToReplaceRegexp, replacementFolder, _collection) {
   // replaces footage of all comp layers with (highres) version
   // from replacementFolder (same file name)

   const collection = _collection || GetProject();
   if (!collection) return;

   if (!replacementFolder) return;

   var comp = null;
   for (var j = 0; j < collection.items.length; j++) {

      comp = collection.item(j + 1);
      if (!IsComp(comp)) continue;
      $.writeln(comp.name);

      var layer = null;
      var layerName = '';
      var repItem = null;
      var layerSrc = null;
      var scale = null;
      var numKeys = 0;
      for (var i = 0; i < comp.numLayers; i++) {
         layer = comp.layer(i + 1);
         layerName = layer.name;
         if (!filesToReplaceRegexp.test(layerName)) continue;
         layerSrc = layer.source;
         if (!IsFootage(layerSrc)) continue;

         repItem = GetItemByName(replacementFolder, layerName);
         if (!repItem) {
            $.writeln('replacement for "' + layerName + '" not found');
            continue;
         }

         $.writeln('   replacing ' + layerName);

         layer.replaceSource(repItem, false);
         scale = layer.transform.scale;
         numKeys = scale.numKeys;
         if (numKeys == 0) {
            scale.setValue(scale.value / 2);
            continue;
         }
         for (var j = 0; j < numKeys; j++) {
            scale.setValueAtKey(j + 1, scale.keyValue(j + 1) / 2);
         }
      }
   }
}

function DebugTest() {
   var _proj = app.project;
   var msg = _proj ? 'Project found' : 'No project found';
   $.writeln(msg);
   if (!_proj) return;
   //_proj.activeItem.time += 25*app.project.activeItem.frameDuration;
   //app.activeViewer.setActive();
   if (!_proj.activeItem) {
      $.writeln('No project active item');
      return;
   }
   //_proj.activeItem.openInViewer();
   //return;
   var comp = _proj.activeItem;
   if (!comp || !(comp instanceof CompItem)) {
      msg = 'No active item or active item is not a comp';
      $.writeln(msg);
      return;
   }

   var layer = comp.selectedLayers[0];
   if (!layer) {
      msg = 'No layer selected';
      $.writeln(msg);
      return;
   }

   var src = layer.source;
   if (!src || !(src instanceof CompItem)) return;

   var playhead = comp.time;
   layer.inPoint = layer.startTime; // Content starts 1 seconds after layer starts
   layer.outPoint = layer.startTime + 2; // Content ends at 2 seconds from layer start
   //$.writeln('in: ' + layer.inPoint + ', dur: ' + src.duration + ', out: ' + layer.outPoint);
   // var cmdId = -1;
   // var cmdStr = 'Reveal Properties with Keyframes';
   // cmdId = app.findMenuCommandId(cmdStr);
   // if (cmdId == -1) { 
   //    msg = 'Command "' + cmdStr + '" id not found';
   //    $.writeln(msg);
   //    return;
   // }
   // app.executeCommand(cmdId);

}

function Test() {

   const proj = GetProject();

   var text = "Hello, world!";
   var regex = /world/;
   if (regex.test(text)) {
      $.writeln("Match found!");
   } else {
      $.writeln("No match found.");
   }
}

//Test();

var highresFolder = GetFolderByName(GetProject(), 'video_highres');
SwapFootageItems(/\.mov$/, highresFolder);
//SwapFootageItems(/\.mov$/, highresFolder, GetFolderByName(GetProject(), 'credits.aep'));

//ResizeToDuration();

// SwapFootageItemsByName(
//    'video_highres',
//    [
//       'star sky',

//    ]);

//DebugTest();

//MoveShapeGroupsToLayers();

// CopyProperties(
//    'rocket',
//    [
//       'star 1',
//       'star 2',
//       'star 3',
//       'sun 1',
//       'sun 2',
//       'sun 3',
//       'sun 4',
//       'ring',
//    ]);