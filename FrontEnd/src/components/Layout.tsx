import { NavLink, Outlet } from 'react-router-dom';

export function Layout() {
  return (
    <div style={{ display: 'flex' }}>
      <aside className="sidebar">
        <div className="sidebar-brand">
          <h1>BeSpoked Bikes</h1>
        </div>

        <nav>
          <div className="nav-section">
            <div className="nav-section-title">SALES</div>
            <NavLink to="/" end className="nav-link">
              Dashboard
            </NavLink>
            <NavLink to="/sales/new" className="nav-link">
              New Sale
            </NavLink>
          </div>

          <div className="nav-section">
            <div className="nav-section-title">MANAGEMENT</div>
            <NavLink to="/products" className="nav-link">
              Products
            </NavLink>
            <NavLink to="/salespersons" className="nav-link">
              Salespersons
            </NavLink>
            <NavLink to="/customers" className="nav-link">
              Customers
            </NavLink>
          </div>

          <div className="nav-section">
            <div className="nav-section-title">REPORTS</div>
            <NavLink to="/sales" end className="nav-link">
              Sales History
            </NavLink>
            <NavLink to="/report" className="nav-link">
              Quarterly Report
            </NavLink>
          </div>
        </nav>
      </aside>

      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
}
