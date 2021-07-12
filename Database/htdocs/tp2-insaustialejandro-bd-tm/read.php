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

	//get value username from table Player and check if it's equal to the variable username
	$namecheckquery = "SELECT username, password FROM player WHERE username ='" . $username ."';";
	//Check if there was any error with the namecheck above
	$namecheck = mysqli_query($connection, $namecheckquery) or die("2: Name Check Failed");

	//if there are more or less results than 0 in the name check, there is something wrong with the name
	if (mysqli_num_rows($namecheck) != 1)
	{
		echo "3: Name Unexistant";
		exit();
	}

	//get data from name check (string array wich contains the values from the Player table)
	$existingInfo = mysqli_fetch_assoc($namecheck);
	echo $existingInfo["username"]."\t".$existingInfo["password"]; //print values
 ?> 