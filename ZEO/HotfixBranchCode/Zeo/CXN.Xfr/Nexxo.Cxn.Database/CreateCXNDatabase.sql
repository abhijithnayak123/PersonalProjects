﻿USE [master]
GO

/****** Object:  Database [CXN]   ******/

IF NOT EXISTS(select * from sys.databases where name = 'CXN')
BEGIN
CREATE DATABASE [CXN] ON  PRIMARY 
( NAME = N'CXN', FILENAME = N'D:\MSSQL_Data\CXN.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CXN_log', FILENAME = N'E:\MSSQL_TransactionLogs\CXN_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)


ALTER DATABASE [CXN] SET COMPATIBILITY_LEVEL = 100


IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CXN].[dbo].[sp_fulltext_database] @action = 'enable'
end


ALTER DATABASE [CXN] SET ANSI_NULL_DEFAULT OFF 


ALTER DATABASE [CXN] SET ANSI_NULLS OFF 


ALTER DATABASE [CXN] SET ANSI_PADDING OFF 


ALTER DATABASE [CXN] SET ANSI_WARNINGS OFF 


ALTER DATABASE [CXN] SET ARITHABORT OFF 


ALTER DATABASE [CXN] SET AUTO_CLOSE OFF 


ALTER DATABASE [CXN] SET AUTO_CREATE_STATISTICS ON 


ALTER DATABASE [CXN] SET AUTO_SHRINK OFF 


ALTER DATABASE [CXN] SET AUTO_UPDATE_STATISTICS ON 


ALTER DATABASE [CXN] SET CURSOR_CLOSE_ON_COMMIT OFF 


ALTER DATABASE [CXN] SET CURSOR_DEFAULT  GLOBAL 


ALTER DATABASE [CXN] SET CONCAT_NULL_YIELDS_NULL OFF 


ALTER DATABASE [CXN] SET NUMERIC_ROUNDABORT OFF 


ALTER DATABASE [CXN] SET QUOTED_IDENTIFIER OFF 


ALTER DATABASE [CXN] SET RECURSIVE_TRIGGERS OFF 


ALTER DATABASE [CXN] SET  DISABLE_BROKER 


ALTER DATABASE [CXN] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 


ALTER DATABASE [CXN] SET DATE_CORRELATION_OPTIMIZATION OFF 


ALTER DATABASE [CXN] SET TRUSTWORTHY OFF 


ALTER DATABASE [CXN] SET ALLOW_SNAPSHOT_ISOLATION OFF 

ALTER DATABASE [CXN] SET PARAMETERIZATION SIMPLE 


ALTER DATABASE [CXN] SET READ_COMMITTED_SNAPSHOT OFF 


ALTER DATABASE [CXN] SET HONOR_BROKER_PRIORITY OFF 


ALTER DATABASE [CXN] SET  READ_WRITE 


ALTER DATABASE [CXN] SET RECOVERY FULL 


ALTER DATABASE [CXN] SET  MULTI_USER 


ALTER DATABASE [CXN] SET PAGE_VERIFY CHECKSUM  


ALTER DATABASE [CXN] SET DB_CHAINING OFF 

END