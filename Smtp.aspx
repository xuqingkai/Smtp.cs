<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Smtp.cs</title>
</head>
<body>
	<%
		SH.Smtp smtp = new SH.Smtp();
		smtp.Send().Json();
	%>
</body>
</html>
