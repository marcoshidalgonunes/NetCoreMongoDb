import React, { Component } from 'react';
import { Book } from './Book';
import ApiBook from '../services/apiBooks';

export class FetchBooks extends Component {
  static displayName = FetchBooks.name;

  constructor(props) {
    super(props);
    this.state = { catalog: [], loading: true };
  }

  componentDidMount() {
    this.populateBookData();
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading data. If results are not shown, the Catalog Service is offline.</em></p>
      : <Book books={this.state.catalog} />;

    return (
      <div>
        <h1 id="tabelLabel" >Book Catalog</h1>
        {contents}
      </div>
    );
  }

  async populateBookData() {
    var url = this.props.url;
    await ApiBook.getAll()
      .then(data => this.setState({ catalog: data, loading: false }))
      .catch(err => console.log(err));
    //const data = await response.json();
    
  }
}
