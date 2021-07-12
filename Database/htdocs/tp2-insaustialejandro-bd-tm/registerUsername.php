	<?php 
	//connection to data base
	$con = mysqli_connect("localhost","root","root","tp2-insaustialejandro-bd-tm");

	if (mysqli_connect_errno()) //if there was an error with the connection
	{
		echo "1: Connection Failed"; //error code #1, fail to connect
		exit();
	}

	//now that connection succeded, create variables
	$username = $_POST["username"];


	//get value username from table player and check if it's equal to the variable username
	$namecheckquery = "SELECT username FROM player WHERE username ='".$username."';";

	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name Check Failed");

	if(mysqli_num_rows($namecheck) > 0)
	{
		echo "3: Name already exists"; //error code #3 name exist cannot register
		exit();
	}

	//add user to the table
		$insertuserquery = "INSERT INTO  player (username) VALUES ('".$username."');";
		mysqli_query($con, $insertuserquery) or die("4: Insert player query failed"); //error code # - insert query failed

		echo ("1");
	 ?>