import React from 'react';
import { Link } from 'react-router-dom';
import '../App.css';

function Navbar() {
  return (
    <nav className="nav-container">
      <h3>FOOTBALL PLAYERS</h3>
      <ul>
        <li><Link to="/">HOME</Link></li>
        <li><Link to="/players">PLAYERS</Link></li>
        <li><Link to="/add-player">ADD PLAYER</Link></li>
      </ul>
    </nav>
  );
}

export default Navbar;
