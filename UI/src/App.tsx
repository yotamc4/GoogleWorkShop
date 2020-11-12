import { Stack } from '@fluentui/react';
import React from 'react';
import './App.css';
import { Home } from './HomePage/Home';
import ButtonAppBar from './LoginBar';


function App() {
  return (
    <div>
      <ButtonAppBar/>
      <Home/>
    </div>
  );
}

export default App;
