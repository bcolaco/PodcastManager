# Podcast Manager
This project is a very basic podcast manager that aims to solve a very specific problem.

[![Build Status](https://travis-ci.org/bcolaco/PodcastManager.svg?branch=master)](https://travis-ci.org/bcolaco/PodcastManager)

## The Problem
I own a 2010 Fiat Bravo and I spend a lot of time driving it.
Also, I like to use my commuting time listening to podcasts.
The car is equipped with a basic entertaining system called Blue&amp;Me, which allows me to plug in a USB thumb drive and play any MP3 files it contains. But it lacks all the remainig features for managing podcasts.

## The Solution
The solution is to add to my USB thumb drive a very simple application that allows me to automate as much as possible the the podcast management to listen while I'm driving.

### The Process
Managing podcasts consists of the following steps:

| # | Location | Description                                      |
|---|----------|--------------------------------------------------|
| 1 | PC       | Edit a list of podcasts you want to listen       |
| 2 | PC       | Download the latest episodes to your thumb drive |
| 3 | Car      | Listen to the downloaded episodes                |
| 4 | PC       | Delete the episodes you have listened            |
Repeat from step 1, 2 or 3

## The Challenge
I've solved this problem with a simple .NET console application.
But I wanted to try and learn .NET Core 2, in Linux, so I decided to port the application to this technology and share it with the world.
