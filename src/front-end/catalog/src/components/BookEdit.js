import React, { Component } from 'react';
import { Link, Redirect } from "react-router-dom";
import ApiBooks from '../services/apiBooks';

class BookEdit extends Component {

    constructor(props) {
      super(props);
      const id = props.match ? props.match.params.id : null;
      this.state = { 
        loading: true,
        id: id, 
        bookName: null,
        author: null,
        category: null,
        price: null, 
        error: null,
        redirect: null
      };
    }

    onAuthorChange = (event) => {
      this.setState({ author: event.target.value });
    }

    onCategoryChange = (event) => {
      this.setState({ category: event.target.value });
    }

    onPriceChange = (event) => {
      this.setState({ price: event.target.value });
    }

    onNameChange = (event) => {
      this.setState({ bookName: event.target.value });
    }

    onCancel = () => {
      this.setState({ redirect: '/'});
    }
  
    componentDidMount() {
      this.populateBook();
    }
    
    render() {
      let content = this.state.loading
        ? <div className='alert alert-light'>Loading data...</div>
        : this.state.error 
           ? <div className='alert alert-danger mt-3 mx-5'>An error happened with Catalog Service: <strong>{this.state.error}</strong><hr/><Link to='/'>Return</Link></div>
           : this.state.redirect 
             ? <Redirect to={this.state.redirect} />
             : this.createForm();
      return (
        <div>
          {content}
        </div>
      )
    }

    getBook() {
      return {
        id: null,
        bookName: this.state.bookName,
        author: this.state.author,
        category: this.state.category,
        price: this.state.price
      }
    }

    async populateBook() {
      const id = this.state.id;
      if (id) {
        await ApiBooks.getById(id)
          .then(response => this.setState({
            loading: false,
            id: response.data.id,
            bookName: response.data.bookName,
            author: response.data.author,
            category: response.data.category,
            price: response.data.price
          }))
          .catch(err => { 
            console.log(err);
            this.setState( { loading: false, error: err.message });
          });
      }
      else {
        this.setState({ loading: false })
      }
    }

    async updateBook() {
      const book = this.getBook();
      book.id = this.state.id;
      await ApiBooks.update(book)
        .then(() => this.setState({ redirect: '/'}))
        .catch(err => { 
          console.log(err);
          const status = err.response.status;
          const message = status === 404 ? 'Book not found' : err.message;
          this.setState({ error: message, loading: false });
      }); 
    }

    createForm() {
      return (
        <div className="container h-100">
          <div className="row h-100 justify-content-center align-items-center">
            <div className="col-10 col-md-8 col-lg-6">
              <h1>{this.action} Book</h1>
              <div className="form-group mt-2">
                <label htmlFor="bookName">Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="bookName"
                  required
                  value={this.state.bookName}
                  onChange={this.onNameChange}
                  name="title"
                />
              </div>
              <div className="form-group mt-2">
                <label htmlFor="author">Author</label>
                <input
                  type="text"
                  className="form-control"
                  id="author"
                  required
                  value={this.state.author}
                  onChange={this.onAuthorChange}
                  name="description"
                />
              </div>
              <div className="form-group mt-2">
                <label htmlFor="category">Category</label>
                <input
                  type="text"
                  className="form-control"
                  id="category"
                  required
                  value={this.state.category}
                  onChange={this.onCategoryChange}
                  name="description"
                />
              </div>
              <div className="form-group mt-2">
                <label htmlFor="price">Price</label>
                <input
                  type="text"
                  className="form-control"
                  id="price"
                  required
                  value={this.state.price}
                  onChange={this.onPriceChange}
                  name="description"
                />
              </div>
              <div className="d-flex justify-content-center">
                <button onClick={this.onSubmit} className="btn btn-primary mt-3 mx-1">
                  Submit
                </button>
                <button onClick={this.onCancel} className="btn btn-outline-secondary mt-3 mx-1">
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </div>
      )  
    }
}

export class BookCreate extends BookEdit {
  static displayName = BookCreate.name;

  constructor(props) {
    super(props);
    this.onSubmit = this.onCreateSubmit;
    this.action = 'New';
  }

  onCreateSubmit = () => {
    this.createBook();
  }

  populateBook() {
    this.setState({ loading: false });
  }

  async createBook() {
    const book = this.getBook();
    await ApiBooks.create(book)
      .then(() => this.setState({ redirect: '/'}))
      .catch(err => { 
        console.log(err);
        this.setState( { error: err.message });
    });
  }
}

export class BookUpdate extends BookEdit {
  static displayName = BookUpdate.name;

  constructor(props) {
    super(props);
    this.onSubmit = this.onUpdateSubmit;
    this.action = 'Update';
  }

  onUpdateSubmit = () => {
    this.updateBook();
  }

  async populateBook() {
    await ApiBooks.getById(this.state.id)
      .then(response => this.setState({
         loading: false,
         id: response.data.id,
         bookName: response.data.bookName,
         author: response.data.author,
         category: response.data.category,
        price: response.data.price
      }))
      .catch(err => { 
        console.log(err);
        this.setState( { loading: false, error: err.message });
    });
  }

  async updateBook() {
    const book = this.getBook();
    book.id = this.state.id;
    await ApiBooks.update(book)
      .then(() => this.setState({ redirect: '/'}))
      .catch(err => { 
        console.log(err);
        const status = err.response.status;
        const message = status === 404 ? 'Book not found' : err.message;
        this.setState({ error: message, loading: false });
    }); 
  }
}

export class BookDelete extends BookEdit {
  static displayName = BookDelete.name;

  constructor(props) {
    super(props);
    this.action = 'Delete';
  }

  onDeleteSubmit = () => {
    this.deleteBook();
  }

  async deleteBook() {
    const book = this.getBook();
    const id = this.state.id;
    await ApiBooks.delete(id)
      .then(() => this.setState({ redirect: '/'}))
	    .catch(err => { 
        console.log(err);
        this.setState( { loading: false, error: err.message });
	  });
  }
  
  createForm() {
    return (
      <div className="container h-100">
        <div className="row h-100 justify-content-center align-items-center">
          <div className="col-10 col-md-8 col-lg-6">
            <h1>Delete Book</h1>
            <div className='alert alert-warning'>
			        Are you sure to delete the book <strong>{this.state.bookName}</strong> from <strong>{this.state.author}</strong>?
			      </div>
            <div className="d-flex justify-content-center">
              <button onClick={this.onCancel} className="btn btn-outline-secondary mt-3 mx-1">
                Cancel
              </button>
              <button onClick={this.onDeleteSubmit} className="btn btn-warning mt-3 mx-1">
                Delete
              </button>
            </div>
          </div>
        </div>
      </div>
    )  
  } 
}
