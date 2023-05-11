import React, { Component } from 'react';
import { Link } from "react-router-dom";

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
                  <th>Name</th>
                  <th>Author</th>
                  <th>Category</th>
                  <th>Price</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {this.books && this.books.map(book =>
                  <tr key={book.Id}>
                    <td>
                      <Link to={{ pathname: `/updatebook/${book.Id}`}}>
                      {book.Name}
                      </Link>
                    </td>
                    <td>{book.Author}</td>
                    <td>{book.Category}</td>
                    <td>{book.Price}</td>
                    <td>
                      <Link to={{ pathname: `/deletebook/${book.Id}`}}>
                        <button
                          className="btn btn-outline-warning py-0"
                          type="button">
                          Delete
                        </button>
                      </Link>
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
        );
    }
}
