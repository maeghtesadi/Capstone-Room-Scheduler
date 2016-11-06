-- phpMyAdmin SQL Dump
-- version 4.6.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 06, 2016 at 09:13 PM
-- Server version: 5.7.14
-- PHP Version: 5.6.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `reservation_system`
--

-- --------------------------------------------------------


-- --------------------------------------------------------

--
-- Table structure for table `room`
--

CREATE TABLE `room` (
  `roomID` int(11) NOT NULL,
  `roomNum` varchar(10) NOT NULL,
  PRIMARY KEY (roomID)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `timeslot`
--

CREATE TABLE `timeslot` (
  `timeslotID` int(11) NOT NULL,
  `roomID` int(11) NOT NULL,
  `hourlyID` enum('8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23') NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (timeslotID),
  FOREIGN KEY (roomID) REFERENCES  room(roomID)
  
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `userID` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `numOfReservations` int(11) NOT NULL,
  `password` varchar(30) NOT NULL,
  
  PRIMARY KEY (userID)
  
  
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------



--
-- Table structure for table `reservation`
--

CREATE TABLE `reservation` (
  `reservationID` int(11) NOT NULL,
  `userID` int(11) NOT NULL,
  `timeSlotID` int(11) NOT NULL,
  `description` text NOT NULL,
  PRIMARY KEY (`reservationID`),
  FOREIGN KEY (userID) REFERENCES  user(userID),
  FOREIGN KEY (timeSlotID) REFERENCES timeslot(timeslotID)
  
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


--
-- Table structure for table `waitsfor`
--

CREATE TABLE `waitsfor` (
  `userID` int(11) NOT NULL,
  `reservationID` int(11) NOT NULL,
  `dateTime` datetime NOT NULL,
  PRIMARY KEY (userID, reservationID),
  FOREIGN KEY (userID) REFERENCES  user(userID),
  FOREIGN KEY (reservationID) REFERENCES  reservation(reservationID)
  
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
