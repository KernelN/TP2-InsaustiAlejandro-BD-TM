<?php
	$con = mysqli_connect("localhost","root","root", "unityclass1");

//
//	check that connection happened

	if (mysqli_connect_errno())
	{ 
		echo "1: Connection failed"; //error code #1 = connection failed
		exit();
	}

	$username = $_POST["name"];
	$score = $_POST["score"];

//check if name exists
$namecheckquery = "SELECT username, score FROM players WHERE username='" . $username . "';";

$namecheck = mysqli_query($con,$namecheckquery) or die ("2: Name check query failed"); //error code #2 - name check query failed

if(mysqli_num_rows($namecheck)!= 1)
{
	echo "3: Nombre Inexistente"; //error code #3 name exist cannot register
	exit();
}

$existingInfo = mysqli_fetch_assoc($namecheck); //gracias a esto se puede separar el name del score

	echo $existingInfo["username"]. "\t" . $existingInfo["score"] ;
 ?>
 