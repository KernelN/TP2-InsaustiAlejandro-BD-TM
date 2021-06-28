<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8">
	<title></title>
</head>
<body>

	<?php 


	//connection to data base
	$connection = mysqli_connect("localhost","root","root","tp2-insaustialejandro-bd-tm");

	if (mysqli_connect_errno()) //if there was an error with the connection
	{
		echo "1: Connection Failed"; //error code #1, fail to connect
		exit();
	}

	//now that connection succeded, create variables
	$username = $_POST["username"];
	$password = $_POST["password"];


	//get value username from table Player and check if it's equal to the variable username
	$namecheckquery = "SELECT username FROM Player WHERE username ='" . $username ."';";

	$namecheck = mysqli_query($connection, $namecheckquery) or die("2: Name Check Failed")

	 ?>

</body>
</html>