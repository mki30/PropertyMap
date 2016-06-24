<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>
<style type="text/css">
</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<body onload="updateClock(); setInterval('updateClock()', 1000 )">
    <form action="http://www.google.co.in" id="cse-search-box">
  <div>
    <input type="hidden" name="cx" value="partner-pub-9629311196237402:9855303491" />
    <input type="hidden" name="ie" value="UTF-8" />
    <input type="text" name="q" size="55" />
    <input type="submit" name="sa" value="Search" />
  </div>
</form>
<script type="text/javascript" src="http://www.google.co.in/coop/cse/brand?form=cse-search-box&amp;lang=en"></script>
</body>
</html>
