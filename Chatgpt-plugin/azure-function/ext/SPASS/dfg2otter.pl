#! /usr/bin/perl

#**************************************************************
#* ********************************************************** *
#* *                                                        * *
#* *          Adds appropriate options for Otter            * *
#* *                                                        * *
#* *                                                        * *
#* $Revision: 1.1 $                                        * *
#* $State: Exp $                                            * *
#* $Date: 2006/03/07 16:19:26 $                             * *
#* $Author: topic $                                       * *
#* *                                                        * *
#* *             Contact:                                   * *
#* *             Christoph Weidenbach                       * *
#* *             MPI fuer Informatik                        * *
#* *             Stuhlsatzenhausweg 85                      * *
#* *             66123 Saarbruecken                         * *
#* *             Email: spass@mpi-inf.mpg.de                * *
#* *             Germany                                    * *
#* *                                                        * *
#* ********************************************************** *
#**************************************************************


# $RCSfile: dfg2otter.pl,v $ 


#############################################################
#
# Perl script for converting DFG input files into
# OTTER input files. The main work is done by the
# c-executable 'dfg2otter'.
#
# Usage:
#
# dfg2otter.pl [ -t] [file1] [file2]
#
# Options:
#
# -t (time): sets timelimit in seconds for OTTER in the converted
#     file (i.e., generates the command 'assign(max_seconds, time).')
#
# Functionality:
#
# The number of file arguments determines where dfg2otter
# takes its input from and directs its output to:
#
# file1 and file2 are given:
#     read from file1, write to file2
#
# file1 is given:
#     read from file1, generate output file file.ot
#
# No file arguments: 
#     input is read from stdin, output written to stdout
#
#
#############################################################

#########################################################
#  Packages
#########################################################

use strict;
# getopts package
use Getopt::Std;
use File::Copy;

#########################################################
# temporaries
#########################################################
my @TOREMOVE = ();

#########################################################
# Defaults
#########################################################

my $DFG2OTTER = "dfg2otter";    # dfg2otterpath
my $OUTFILESUFFIX = ".ot";      # generic output file suffix
my $ERRORFILESUFFIX = ".error"; # generic error file suffix

#########################################################
# options
#########################################################

# define default options
my %options = ( t => 10 );    # default: 10 seconds timeout for proof checker

# read options
&getopts("t:", \%options);

# shortcuts for options
my $timelimit = $options{'t'};

#########################################################
# MAIN
#########################################################
$SIG{INT} = \&catchint;         # set up interrupt handler
# try to get dfg2otter path from environment
# otherwise default is kept
if (defined($ENV{'DFG2OTTER'})) { 
  $DFG2OTTER = $ENV{'DFG2OTTER'};
}


# check that there are at most 2 arguments
if (@ARGV > 2) { die "Too many arguments.\n";}
#########################################################
# if there are no arguments, just call the original
#########################################################
if (@ARGV == 0) {
  system("$DFG2OTTER -Stdin");
}

#########################################################
# if one argument is given, use it as input file and
# generate file.ot output file 
#########################################################
my ($in, $out);
if (@ARGV == 1) {
  $in  = $ARGV[0];
  $out = $in."_".$$."_".$OUTFILESUFFIX;
  print `$DFG2OTTER -Stdin < $in > $out`;
}

#########################################################
# if two arguments are given, generate output filename
# first, write parameters to it, and then append dfg2otter
# output
#########################################################
my ($errorfile, $parout, $convout, @conv_res, $errorlines);

if (@ARGV == 2) { 
  
  $in  = $ARGV[0];
  $out = $ARGV[1];
  $errorfile = $out."_".$$."_".$ERRORFILESUFFIX;
  
  # print parameters
  $parout  = gettmp($out);
  open(CONV, ">$parout") or die "dfg2otter.pl: could not open '$in': $!"; 
  select CONV;
  &print_parameters();
  close CONV;
  
  # convert SPASS file
  $convout = gettmp($out);
  @conv_res = `$DFG2OTTER $in $convout 2>$errorfile`;
  
  # echo STDERR, remove files if errors occured
  # (that is if $errorfile is not empty)
  if (-e $errorfile) {
    unless (open(EF, "<$errorfile")) {
      cleanup();
      die "dfg2otter.pl: error opening '$errorfile': $!";
    }
    $errorlines = 0;
    while (<EF>) {
      print STDERR;
      $errorlines++;
    }
    close(EF);
    unlink($errorfile);
    if ($errorlines) { 
      unlink($out);
    }
  }
  
  # the following lines copy the file $parout to $out, and then append $convout,
  # like the UNIX command 'cat $parout $convout > $out'
  copy($parout, $out) or die "Could not copy $parout to $out: $!";
  open (IN, "$convout") or die "Could not open $convout: $!";
  open(APPEND, ">>$out") or die "Could not open $out: $!";
  while (<IN>) {
    print APPEND;
  }
  close(APPEND);
  close(IN);
  
  cleanup();
}


####################################################
# INPUT  : A signal name
# EFFECTS: Remove temporaries and reports interrupt 
####################################################
sub catchint {
  my $signame = shift;
  cleanup();
  die "\n\ndfg2otter.pl: caught signal $signame. Cleaning up."; 
}

####################################################
# EFFECTS: Remove temporaries
#####################################################
sub cleanup {
  unlink @TOREMOVE;
}

####################################################
# EFFECTS: Print otter default parameters to stdout
####################################################
sub print_parameters {
  print "set(process_input).\n";
  print "set(binary_res).\n";
  print "set(factor).\n";
  print "set(hyper_res).\n";
  print "assign(pick_given_ratio, 4).\n";
  print "clear(print_kept).\n";
  print "clear(print_new_demod).\n";
  print "clear(print_back_demod).\n";
  print "clear(print_back_sub).\n";
  print "set(knuth_bendix).\n";
  
  print "set(para_from).\n";
  print "set(para_into).\n";
  print "set(para_from_right).\n";
  print "set(para_into_right).\n";
  print "set(para_from_left).\n";
  print "set(para_into_left).\n";
  print "set(para_from_vars).\n";
  print "set(back_demod).\n";
  print "set(auto).\n";
  print "assign(max_seconds, $timelimit).\n";
}

####################################################
# INPUT:  A filename
# OUTPUT: A similar filename that is unique 
####################################################
sub gettmp {
  my ($file) = @_;
  my ($cnt)  = 0;
  
  while (-e $file."$cnt") {
    $cnt++;
  }
  $file = $file."$cnt"; 
  unshift(@TOREMOVE, $file); 
  
  return($file);
}
