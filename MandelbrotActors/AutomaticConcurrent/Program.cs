// Copyright 2009-2010 Oliver Sturm <oliver@sturmnet.org> All rights reserved. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutomaticConcurrent {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main( ) {
      Application.EnableVisualStyles( );
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1( ));
    }
  }
}
