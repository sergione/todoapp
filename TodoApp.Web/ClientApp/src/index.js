import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import ApolloClient from "apollo-boost";
import gql from "graphql-tag";
import { ApolloProvider } from "react-apollo";

import App from './App';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const client = new ApolloClient({
    uri: "http://localhost:5000/graphql"
});


client
    .query({
        query: gql`
      {
        todos {
          id
          description
          complete
        }
      }
    `
    })
    .then(result => console.log(result));

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <ApolloProvider client={client}>
      <App />
    </ApolloProvider>
  </BrowserRouter>,
  rootElement);

registerServiceWorker();
