<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Map.ascx.cs" Inherits="UserControl_Map" %>
<script src="../js/markerwithlabel_packed.js"></script>
<script src="../js/PolyEdit.js"></script>
<div>
    <input id="chkZoomLock" type="checkbox" />Zoom Lock 
    <input id="chkPanLock" type="checkbox" />Pan Lock 
    <input id="chkAutoImport" type="checkbox" />Auto import
    <input type="text" id="txtAddress" /><input type="button" value="Find" onclick="FindAddress()" />
    <input type="button" value="Reset" onclick="Reset()" />
    <input type="checkbox" id="chkCreateArea"  />
    Create Area
    <input type="text" value="" id="txtLat" />
    <input type="text" value="" id="txtLng" />
    <input type="text" value="" id="txtPolyPoints" />
</div>
<div id="map_canvas" style="width: 100%; height: 800px">
</div>
