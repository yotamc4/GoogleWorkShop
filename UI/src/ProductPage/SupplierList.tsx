import * as React from 'react';
import { DetailsList, IColumn, IDetailsColumnStyles } from 'office-ui-fabric-react/lib/DetailsList';
import { Stack } from 'office-ui-fabric-react';
import { ActionButton, IIconProps } from 'office-ui-fabric-react';
import { IStackStyles, mergeStyleSets, SelectionMode, ProgressIndicator, PrimaryButton, DefaultButton } from '@fluentui/react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
export interface ISuppliersListItem {
  key: number;
  name: string;
  value: number;
  Price: number;
  Minimum: number;
  Date: string;
  ProgressBar: JSX.Element;
}

export interface ISuppliersListState {
  items: ISuppliersListItem[];
}

const classNames = mergeStyleSets({
  fileIconCell: {
    marginLeft: '1.4rem',
  },
  fileIconCell2: {
    marginLeft: '-2rem',
  }
});

export let _allItems:ISuppliersListItem[] = [];

export let _columns:IColumn[] = [];

for (let i = 0; i < 200; i++) {
  _allItems.push({
    key: i,
    name: 'KSP computers and cellular',
    value: i,
    Price: 10000,
    Minimum: i+3,
    Date: Date(),
    ProgressBar: <ProgressIndicator label="70 units to complete" percentComplete={0.4} />,
  });
}

_columns = [
  { key: 'column1', name: 'Supplier name', fieldName: 'name', minWidth: 100, maxWidth: 150, isResizable: true },
  { key: 'column2' , name: 'Price', fieldName: 'Price', minWidth: 70, maxWidth: 120, isResizable: true, styles:{cellTitle:{marginLeft:'1.3rem'}},className: classNames.fileIconCell },
  { key: 'column3', name: 'Minimum units', fieldName: 'Minimum', minWidth: 70, maxWidth: 120, isResizable: true },
  { key: 'column5', name: 'Date', fieldName: 'Date', minWidth: 140, maxWidth: 140, isResizable: true ,className: classNames.fileIconCell2},
  { key: 'column6', name: 'ProgressBar', fieldName: 'ProgressBar', minWidth: 100, maxWidth: 100, isResizable: true ,styles:{cellTitle:{marginLeft:'1rem'}}},
];

export const addIcon: IIconProps = { iconName: 'Add' };

export const stackStyles: Partial<IStackStyles> = { root: { width: '50rem' } };

export const detailsListStyles: Partial<IDetailsColumnStyles> = { root: { textAlign: 'right' } };



export const SuppliersList: React.FunctionComponent = () => {
  const [open, setOpen] = React.useState(false);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };
    return (
      <Stack styles={stackStyles}>
          <ActionButton  iconProps={addIcon} allowDisabledFocus disabled={false} checked={false} onClick={handleClickOpen}>
            Add a new proposal
          </ActionButton>
          <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
        <DialogTitle >{"New proposal"}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            Let Google help apps determine location. This means sending anonymous location data to
            Google, even when no apps are running.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
        <DefaultButton
        onClick={handleClose}
            text="Cancel"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            // checked={checked}
          />
          <PrimaryButton
          onClick={handleClose}
            text="Send"
            //onClick={_alertClicked}
            allowDisabledFocus
            //disabled={disabled}
            //checked={checked}
          />
        </DialogActions>
      </Dialog>
          <>
    </>
          <DetailsList
            items={_allItems}
            columns={_columns}
            selectionMode={SelectionMode.none}
          />
      </Stack>
    );
  
}