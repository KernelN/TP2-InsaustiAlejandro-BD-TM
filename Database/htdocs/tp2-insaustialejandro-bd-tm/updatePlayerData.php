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
	$score = $_POST["score"];
	$deaths = $_POST["deaths"];


	//get value username from table Player and check if it's equal to the variable username
	$namecheckquery = "SELECT username FROM player WHERE username ='".$username."';";
	//Check if there was any error with the namecheck above
	$namecheck = mysqli_query($connection, $namecheckquery) or die("2: Name Check Failed");

	//if there are more or less results than 0 in the name check, there is something wrong with the name
	if (mysqli_num_rows($namecheck) != 1)
	{
		echo "3: Name Unexistant";
		exit();
	}

	//update user data in table
	$updateuserquery = 
	"UPDATE player SET score ='".$score."', deaths ='".$deaths."' WHERE username = '".$username."';";
	
	//check if data update was succesful
	mysqli_query($connection, $updateuserquery) or die("4: Update user query failed");

	echo "1";

	 ?>

</body>
</html>