import * as React from "react";
import * as FormsStyles from "../FormStyles/FormsStyles";
import { Stack, Dropdown } from "office-ui-fabric-react";
import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  IDatePickerStrings,
  IDropdownOption,
  PrimaryButton,
  Separator,
  Text,
  TextField,
  Image,
  MessageBar,
  MessageBarType,
  Spinner,
} from "@fluentui/react";
import { CategoriesMap } from "../HomePage/Model/Categories";
import { newProductRequest } from "../Modal/ProductDetails";
import { NewBidRequest } from "../Modal/GroupDetails";
import { submitNewGroupForm } from "../Services/BidsControllerService";
import { useHistory } from "react-router";

export const NewGroupBuyingForm: React.FunctionComponent = () => {
  const [productDetails, setProductDetails] = React.useReducer<
    (
      prevState: Partial<newProductRequest>,
      state: Partial<newProductRequest>
    ) => newProductRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      Name: "",
      Image: undefined,
      Description: "",
    }
  );

  const [bidRequest, setBidRequest] = React.useReducer<
    (
      prevState: Partial<NewBidRequest>,
      state: Partial<NewBidRequest>
    ) => NewBidRequest
  >(
    (prevState: any, state: any) => ({
      ...prevState,
      ...state,
    }),
    {
      OwnerId: "1",
      Category: "",
      SubCategory: "",
      ExpirationDate: undefined,
      MaxPrice: 0,
      Product: undefined,
    }
  );

  const [
    allRequiredFieldsAreFulfilled,
    SetAllRequiredFieldsAreFulfilled,
  ] = React.useState<boolean>(false);

  const [requestInProcess, setRequestInProcess] = React.useState<boolean>(
    false
  );

  const [errorMessage, setErrorMessage] = React.useState<string>();
  const urlHistory = useHistory();

  React.useEffect(() => {
    const allRequiredFieldsAreFulfilledObjectTemp: boolean =
      Object.values(bidRequest).every((el) => el) &&
      !!productDetails.Description &&
      !!productDetails.Name;

    SetAllRequiredFieldsAreFulfilled(allRequiredFieldsAreFulfilledObjectTemp);
  }, [bidRequest, productDetails]);

  function onDropdownChange(
    event: React.FormEvent<HTMLDivElement>,
    option?: IDropdownOption
  ): void {
    const id: string | undefined = option?.id as string;
    const text: string | undefined = option?.text!;
    setBidRequest({ [id]: text });
  }

  const onTextFieldChange = (
    setStateFunction: Function,
    event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>,
    newValue?: string,
    convertToNumber?: boolean
  ): void => {
    const updatedValue: string | number | undefined = convertToNumber
      ? Number(newValue)
      : newValue;

    setStateFunction({
      [(event.target as HTMLInputElement).id]: updatedValue,
    });
  };

  // TODO - Move the call to separate file (Services controllers)
  const onSubmitForm = async (): Promise<void> => {
    try {
      setRequestInProcess(true);
      await submitNewGroupForm(bidRequest);
      urlHistory.push(`/`);
    } catch {
      setRequestInProcess(false);
      setErrorMessage(
        "An error occurred while trying to create your group. Please try again later."
      );
    }
  };

  return (
    <Stack horizontalAlign={"center"}>
      <Text
        block={true}
        className="Bold"
        styles={FormsStyles.headerStyle}
        variant="xLargePlus"
      >
        New suggestion for Group buying
      </Text>
      <Stack
        styles={{ root: { width: "40%" } }}
        tokens={FormsStyles.verticalGapStackTokens}
      >
        <Separator styles={{ root: { width: "100%" } }} />
        {errorMessage && (
          <MessageBar
            messageBarType={MessageBarType.error}
            onDismiss={() => setErrorMessage("")}
          >
            {errorMessage}
          </MessageBar>
        )}
        <TextField
          id="Name"
          label="Product's name"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
            setBidRequest({ Product: productDetails });
          }}
          required
        />
        <Dropdown
          id="Category"
          onChange={onDropdownChange}
          label={"Product's Category"}
          styles={FormsStyles.dropdownStyles}
          options={Array.from(CategoriesMap.keys()).map((categoryName) => ({
            key: categoryName,
            id: "Category",
            text: categoryName,
          }))}
          required
        ></Dropdown>
        <Dropdown
          id="SubCategory"
          label={"Product's Sub Category"}
          onChange={onDropdownChange}
          styles={FormsStyles.dropdownStyles}
          options={
            bidRequest.Category
              ? Array.from(
                  CategoriesMap.get(bidRequest.Category)!.map(
                    (subCategory) => ({
                      key: subCategory,
                      id: "SubCategory",
                      text: subCategory,
                    })
                  )
                )
              : []
          }
          required
        ></Dropdown>
        <TextField
          id="MaxPrice"
          onChange={(event, newValue) =>
            onTextFieldChange(setBidRequest, event, newValue)
          }
          label="Maximum price"
          onGetErrorMessage={(value) =>
            value === "" || value.match(/^[0-9]+$/) != null
              ? ""
              : "Only numbers allowed"
          }
          styles={{ root: { width: FormsStyles.inputWidth } }}
          suffix="₪"
          required
        />
        <DatePicker
          id="ExpirationDate"
          onSelectDate={(date: Date | null | undefined): void => {
            setBidRequest({
              ExpirationDate: date,
            });
          }}
          value={bidRequest.ExpirationDate ?? undefined}
          label="Set expiration date for the group"
          firstDayOfWeek={DayOfWeek.Sunday}
          strings={DayPickerStrings}
          placeholder="Select a date"
          calendarProps={{
            strings: DayPickerStrings,
            minDate: new Date(2020, 12, 12, 0, 0, 0, 0),
          }}
          styles={{ root: { width: FormsStyles.inputWidth } }}
          isRequired
        />
        <TextField
          id="Description"
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
            setBidRequest({ Product: productDetails });
          }}
          label="Description"
          multiline
          autoAdjustHeight
          styles={{ root: { width: FormsStyles.inputWidth } }}
          required
        />
        <TextField
          id="Image"
          label="Enter URL image for reference"
          styles={{ root: { width: FormsStyles.inputWidth } }}
          onChange={(event, newValue) => {
            onTextFieldChange(setProductDetails, event, newValue);
            setBidRequest({ Product: productDetails });
          }}
        />
        {productDetails?.Image && (
          <Image
            src={productDetails.Image}
            id="target"
            width={"25rem"}
            height={"20rem"}
          />
        )}
        <Separator styles={{ root: { width: FormsStyles.inputWidth } }} />
        <Stack
          horizontal
          tokens={FormsStyles.horizontalGapStackTokens}
          styles={{ root: { margin: "auto" } }}
        >
          <DefaultButton text="Cancel" href={"/"} />
          <Stack horizontal tokens={{ childrenGap: "1rem" }}>
            {requestInProcess && <Spinner />}
            <PrimaryButton
              text="Send"
              onClick={onSubmitForm}
              disabled={!allRequiredFieldsAreFulfilled}
            />
          </Stack>
        </Stack>
      </Stack>
    </Stack>
  );
};

const DayPickerStrings: IDatePickerStrings = {
  months: [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ],

  shortMonths: [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ],

  days: [
    "Sunday",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
  ],

  shortDays: ["S", "M", "T", "W", "T", "F", "S"],

  goToToday: "Go to today",
  prevMonthAriaLabel: "Go to previous month",
  nextMonthAriaLabel: "Go to next month",
  prevYearAriaLabel: "Go to previous year",
  nextYearAriaLabel: "Go to next year",
  closeButtonAriaLabel: "Close date picker",
  monthPickerHeaderAriaLabel: "{0}, select to change the year",
  yearPickerHeaderAriaLabel: "{0}, select to change the month",
};
