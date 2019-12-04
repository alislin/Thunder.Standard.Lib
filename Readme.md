# Thunder.Standard.Lib

[![Build Status](https://dev.azure.com/aideePub/Thunder.Blazor/_apis/build/status/alislin.Thunder.Standard.Lib?branchName=master)](https://dev.azure.com/aideePub/Thunder.Blazor/_build/latest?definitionId=7&branchName=master) ![Nuget (with prereleases)](https://img.shields.io/nuget/v/thunder.standard.lib)

Basic lib for standard.
Inculde list:

- Extensions
  - Enum  
    - Description to string
    - value to enum
    - string to enum
  - Json string and object convert tow way
  - List to ObservableCollection
  - INotifyPropertyChanged automatic convert property name
  - Object copy property to new Object (deep copy)
  - String 
    - pick number in string to list
    - check string IsNull?
    - Clip convert to string list by break or tab or space
    - Fuzzy search string use Like
    - Wildcard search string use KeyLike,wildcard = "_"
- Compress
  - Include gzip,sharpzip,lz4
- LargeObject
Package object,small transfer data.
- SelectOption
- ApiClient  
Easy to webapi by use httpclient,Include package object to largeobject