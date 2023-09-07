using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Craftify.RevitDatabaseExtensions;

/// <summary>
/// This class is used for adding extension methods to a Document class, <see cref="Document"/>
/// </summary>
public static class DocumentExtensions
{
    /// <summary>
    /// It returns elements by a given type
    /// </summary>
    /// <typeparam name="TElement">The element type</typeparam>
    /// <param name="document">Document</param>
    /// <param name="validate">A delegate for validation</param>
    /// <returns>Elements</returns>
    public static IEnumerable<TElement> GetElementsByType<TElement>(this Document document, Func<TElement, bool>? validate = null)
        where TElement : Element
    {

        validate ??= (_ => true);
        var elements = new FilteredElementCollector(document)
            .OfClass(typeof(TElement))
            .Cast<TElement>()
            .Where(e => validate(e));
        return elements;

    }


    /// <summary>
    /// It returns an element by a given name
    /// </summary>
    /// <typeparam name="TElement">The element type</typeparam>
    /// <param name="document">Document</param>
    /// <param name="name">The element name</param>
    /// <returns>The element</returns>
    /// <exception cref="ArgumentNullException">Thrown in case of the absence of the element</exception>
    public static TElement GetElementByName<TElement>(this Document document, string name)
        where TElement : Element
    {
        var element = new FilteredElementCollector(document)
            .OfClass(typeof(TElement))
            .FirstOrDefault(e => e.Name == name);
        if (element is null)
        {
            throw new ArgumentNullException($"The element of the given name : {name} is not present in a document");
        }
        return (TElement)element;
    }


    /// <summary>
    /// Returns the elements by types
    /// </summary>
    /// <typeparam name="TElement1">The first element type</typeparam>
    /// <typeparam name="TElement2">The second element type</typeparam>
    /// <param name="document">Document</param>
    /// <returns>The elements</returns>
    public static IEnumerable<Element> GetElementsByTypes<TElement1, TElement2>(
        this Document document)
        where TElement1 : Element
        where TElement2 : Element
    {
        var types = new List<Type>()
        {
            typeof(TElement1),
            typeof(TElement2)
        };
        var multiClassFilter = new ElementMulticlassFilter(types);
        return new FilteredElementCollector(document)
            .WherePasses(multiClassFilter);

    }

    /// <summary>
    /// Returns the elements by types
    /// </summary>
    /// <typeparam name="TElement1">The first element type</typeparam>
    /// <typeparam name="TElement2">The second element type</typeparam>
    /// <typeparam name="TElement3">The third element type</typeparam>
    /// <param name="document"></param>
    /// <returns>The elements</returns>
    public static IEnumerable<Element> GetElementsByTypes<TElement1, TElement2, TElement3>(
        this Document document)
        where TElement1 : Element
        where TElement2 : Element
        where TElement3 : Element
    {
        var types = new List<Type>()
        {
            typeof(TElement1),
            typeof(TElement2),
            typeof(TElement3),
        };
        var multiClassFilter = new ElementMulticlassFilter(types);
        return new FilteredElementCollector(document)
            .WherePasses(multiClassFilter);

    }


    /// <summary>
    /// Returns the elements by types
    /// </summary>
    /// <typeparam name="TElement1">The first element type</typeparam>
    /// <typeparam name="TElement2">The second element type</typeparam>
    /// <typeparam name="TElement3">The third element type</typeparam>
    /// <typeparam name="TElement4">The forth element type</typeparam>
    /// <param name="document"></param>
    /// <returns>The elements</returns>
    public static IEnumerable<Element> GetElementsByTypes<TElement1, TElement2, TElement3, TElement4>(
        this Document document)
        where TElement1 : Element
        where TElement2 : Element
        where TElement3 : Element
        where TElement4 : Element
    {
        var types = new List<Type>()
        {
            typeof(TElement1),
            typeof(TElement2),
            typeof(TElement3),
            typeof(TElement4)
        };
        var multiClassFilter = new ElementMulticlassFilter(types);
        return new FilteredElementCollector(document)
            .WherePasses(multiClassFilter);

    }
    /// <summary>
    /// It returns elements by types
    /// </summary>
    /// <param name="document">Document</param>
    /// <param name="types">Types</param>
    /// <returns>The elements</returns>
    public static IEnumerable<Element> GetElementsByTypes(
        this Document document,
        params Type[] types)
    {
        if (!types.Any()) throw new ArgumentNullException("There are no types");
        var multiClassFilter = new ElementMulticlassFilter(types);
        return new FilteredElementCollector(document)
            .WherePasses(multiClassFilter);
    }

    /// <summary>
    /// It returns elements by types
    /// </summary>
    /// <param name="document">Document</param>
    /// <param name="types">Types</param>
    /// <returns>The elements</returns>
    public static IEnumerable<Element> GetElementsByTypes(
        this Document document,
        List<Type> types)
    {
        var multiClassFilter = new ElementMulticlassFilter(types);
        return new FilteredElementCollector(document)
            .WherePasses(multiClassFilter);
    }

    public static void Run(
        this Document document, Action doAction, string transactionName = "Default transaction name")
    {
        using (var transaction = new Transaction(document, transactionName))
        {
            transaction.Start();
            doAction.Invoke();
            transaction.Commit();
        }
    }

    public static TReturn Run<TReturn>(
        this Document document, Func<TReturn> doAction, string transactionName = "Default transaction name")
    {
        using var transaction = new Transaction(document, transactionName);
        transaction.Start();
        var output = doAction.Invoke();
        transaction.Commit();
        return output;
    }
    public static TElement GetElement<TElement>(this Document document, ElementId elementId)
        where TElement : Element
    {
        return (TElement)document.GetElement(elementId);
    }
}