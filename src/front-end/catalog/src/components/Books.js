import React, { Component } from 'react';

export class Books extends Component {
    constructor(catalog) {
        super();
        this.books = catalog.books.data;
    }
    render() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Name</th>
                  <th>Author</th>
                  <th>Category</th>
                  <th>Price</th>
                </tr>
              </thead>
              <tbody>
                {this.books && this.books.map(book =>
                  <tr key={book.id}>
                    <td>{book.id}</td>
                    <td>{book.bookName}</td>
                    <td>{book.author}</td>
                    <td>{book.category}</td>
                    <td>{book.price}</td>
                  </tr>
                )}
              </tbody>
            </table>
        );
    }
}
