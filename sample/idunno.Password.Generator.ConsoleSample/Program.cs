// Copyright (c) Barry Dorrans. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using idunno.Password;

var passwordGenerator = new PasswordGenerator();
Console.WriteLine(passwordGenerator.Generate(10, 2, 1));
Console.ReadLine();